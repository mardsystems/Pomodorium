using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pomodorium.Data;
using System.DomainModel.Storage;

namespace Pomodorium.Features.Storage
{
    public class CosmosStore : IAppendOnlyStore
    {
        private readonly CosmosClient _cosmosClient;

        private readonly Container _container;

        private readonly ILogger<CosmosStore> _logger;

        public CosmosStore(
            CosmosClient cosmosClient,
            IOptions<CosmosOptions> optionsInterface,
            ILogger<CosmosStore> logger)
        {
            _cosmosClient = cosmosClient;

            var options = optionsInterface.Value;

            _container = _cosmosClient.GetDatabase(options.Database).GetContainer("EventStore");

            _logger = logger;
        }

        public async Task<IEnumerable<EventRecord>> ReadRecords(long maxCount)
        {
            int count;

            if (maxCount > int.MaxValue)
            {
                count = int.MaxValue;
            }
            else
            {
                count = (int)maxCount;
            }

            //var sort = Builders<EventRecord>.Sort
            //    .Ascending(x => x.Name)
            //    .Ascending(x => x.Version);

            var query = new QueryDefinition(
                query: "SELECT * FROM EventStore e");

            using FeedIterator<EventRecord> feed = _container.GetItemQueryIterator<EventRecord>(
                queryDefinition: query
            );

            List<EventRecord> events = new();

            double requestCharge = 0d;

            while (feed.HasMoreResults)
            {
                FeedResponse<EventRecord> response = await feed.ReadNextAsync();

                foreach (EventRecord @event in response)
                {
                    events.Add(@event);
                }

                requestCharge += response.RequestCharge;
            }

            return events;
        }

        public async Task<IEnumerable<EventRecord>> ReadRecords(string name, long afterVersion, long maxCount)
        {
            int count;

            if (maxCount > int.MaxValue)
            {
                count = int.MaxValue;
            }
            else
            {
                count = (int)maxCount;
            }

            var query = new QueryDefinition(
                query: "SELECT * FROM EventStore e WHERE e.Name = @name AND e.Version > @afterVersion ORDER BY e.Version")
                .WithParameter("@name", name)
                .WithParameter("@afterVersion", afterVersion);

            try
            {
                //var events = await _container.Find(filter).Sort(sort).ToListAsync();

                using FeedIterator<EventRecord> feed = _container.GetItemQueryIterator<EventRecord>(
                    queryDefinition: query
                );

                List<EventRecord> events = new();

                double requestCharge = 0d;

                while (feed.HasMoreResults)
                {
                    FeedResponse<EventRecord> response = await feed.ReadNextAsync();

                    foreach (EventRecord @event in response)
                    {
                        events.Add(@event);
                    }

                    requestCharge += response.RequestCharge;
                }

                return events;
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(), ex, "ReadRecords");

                throw;
            }
        }

        public async Task<EventRecord> Append(string name, string typeName, DateTime date, byte[] data, long expectedVersion = -1)
        {
            var version = await GetMaxVersion(name, expectedVersion);

            var @event = new EventRecord(name, version + 1, date, typeName, data);

            await _container.CreateItemAsync(@event);

            return @event;
        }

        public async Task Append(EventRecord tapeRecord)
        {
            var version = await GetMaxVersion(tapeRecord.Name, -1);

            var @event = new EventRecord(tapeRecord.Name, version + 1, tapeRecord.Date, tapeRecord.TypeName, tapeRecord.Data);

            await _container.CreateItemAsync(@event);
        }

        private async Task<long> GetMaxVersion(string name, long expectedVersion)
        {
            var query = new QueryDefinition(
                query: "SELECT MAX(e.Version) AS MaxVersion FROM EventStore e WHERE e.Name = @name GROUP BY e.Name")
                .WithParameter("@name", name);

            long version;

            try
            {

                using FeedIterator<MaxVersionTuple> feed = _container.GetItemQueryIterator<MaxVersionTuple>(
                    queryDefinition: query
                );

                List<MaxVersionTuple> versions = new();

                double requestCharge = 0d;

                while (feed.HasMoreResults)
                {
                    FeedResponse<MaxVersionTuple> response = await feed.ReadNextAsync();

                    foreach (MaxVersionTuple item in response)
                    {
                        versions.Add(item);
                    }

                    requestCharge += response.RequestCharge;
                }

                version = versions.Select(x => x.MaxVersion).FirstOrDefault();
            }
            catch (Exception)
            {
                version = -1;
            }

            //var version = _events
            //    .Where(x => x.Name == name)
            //    .Select(x => x.Version)
            //    .DefaultIfEmpty()
            //    .Max();

            if (expectedVersion != -1)
            {
                if (version != expectedVersion)
                {
                    throw new AppendOnlyStoreConcurrencyException(version, expectedVersion, name);
                }
            }

            return version;
        }

        public void Close()
        {

        }

        public void Dispose()
        {

        }
    }

    public class MaxVersionTuple
    {
        public long MaxVersion { get; set; }
    }
}
