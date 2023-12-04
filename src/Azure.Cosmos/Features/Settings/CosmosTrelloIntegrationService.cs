using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pomodorium.Data;
using Pomodorium.Models;
using Pomodorium.Repositories;

namespace Pomodorium.Features.Settings;

public class CosmosTrelloIntegrationService : ITrelloIntegrationRepository
{
    private readonly CosmosClient _cosmosClient;

    private readonly Container _container;

    private readonly ILogger<CosmosTrelloIntegrationService> _logger;

    public CosmosTrelloIntegrationService(
        CosmosClient cosmosClient,
        IOptions<CosmosOptions> optionsInterface,
        ILogger<CosmosTrelloIntegrationService> logger)
    {
        _cosmosClient = cosmosClient;

        var options = optionsInterface.Value;

        _container = _cosmosClient.GetDatabase(options.Database).GetContainer("TrelloIntegrationContainer");

        _logger = logger;
    }

    public async Task<IEnumerable<TrelloIntegration>> GetTrelloIntegrationList(TrelloIntegration criteria = default, CancellationToken cancellationToken = default)
    {
        var tfsIntegrationList = new List<TrelloIntegration>();

        var query = new QueryDefinition(
            query: "SELECT * FROM TrelloIntegrationContainer p");

        using FeedIterator<TrelloIntegration> feed = _container.GetItemQueryIterator<TrelloIntegration>(queryDefinition: query);

        double requestCharge = 0d;

        while (feed.HasMoreResults)
        {
            FeedResponse<TrelloIntegration> response = await feed.ReadNextAsync();

            foreach (TrelloIntegration @event in response)
            {
                tfsIntegrationList.Add(@event);
            }

            requestCharge += response.RequestCharge;
        }

        _logger.LogInformation($"Request charge:\t{requestCharge:0.00}");

        return tfsIntegrationList;
    }

    public async Task<TrelloIntegration> CreateTrelloIntegration(TrelloIntegration tfsIntegration, CancellationToken cancellationToken = default)
    {
        var response = await _container.CreateItemAsync(item: tfsIntegration, partitionKey: new PartitionKey(tfsIntegration.Id.ToString()));

        _logger.LogInformation($"Request charge:\t{response.RequestCharge:0.00}");

        var tfsIntegrationCreated = response.Resource;

        return tfsIntegrationCreated;
    }

    public async Task<TrelloIntegration> GetTrelloIntegration(Guid id, CancellationToken cancellationToken = default)
    {
        ItemResponse<TrelloIntegration> response = await _container.ReadItemAsync<TrelloIntegration>(
                id: id.ToString(),
                partitionKey: new PartitionKey(id.ToString())
            );

        var tfsIntegration = response.Resource;

        _logger.LogInformation($"Request charge:\t{response.RequestCharge:0.00}");

        return tfsIntegration;
    }

    public async Task<TrelloIntegration> UpdateTrelloIntegration(TrelloIntegration tfsIntegration, CancellationToken cancellationToken = default)
    {
        var response = await _container.UpsertItemAsync(tfsIntegration);

        _logger.LogInformation($"Request charge:\t{response.RequestCharge:0.00}");

        var tfsIntegrationUpdated = response.Resource;

        return tfsIntegrationUpdated;
    }

    public async Task DeleteTrelloIntegration(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _container.DeleteItemAsync<TrelloIntegration>(id.ToString(), new PartitionKey(id.ToString()));

        _logger.LogInformation($"Request charge:\t{response.RequestCharge:0.00}");
    }
}
