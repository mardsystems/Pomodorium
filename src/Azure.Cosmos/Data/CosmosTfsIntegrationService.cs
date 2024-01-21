using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pomodorium.Models;
using Pomodorium.Repositories;

namespace Pomodorium.Data;

public class CosmosTfsIntegrationService : ITfsIntegrationRepository
{
    private readonly CosmosClient _cosmosClient;

    private readonly Container _container;

    private readonly ILogger<CosmosTfsIntegrationService> _logger;

    public CosmosTfsIntegrationService(
        CosmosClient cosmosClient,
        IOptions<CosmosOptions> optionsInterface,
        ILogger<CosmosTfsIntegrationService> logger)
    {
        _cosmosClient = cosmosClient;

        var options = optionsInterface.Value;

        _container = _cosmosClient.GetDatabase(options.Database).GetContainer("TfsIntegrationContainer");

        _logger = logger;
    }

    public async Task<IEnumerable<TfsIntegration>> GetTfsIntegrationList(TfsIntegration? criteria = default, CancellationToken cancellationToken = default)
    {
        var tfsIntegrationList = new List<TfsIntegration>();

        var query = new QueryDefinition(
            query: "SELECT * FROM TfsIntegrationContainer p");

        using FeedIterator<TfsIntegration> feed = _container.GetItemQueryIterator<TfsIntegration>(queryDefinition: query);

        double requestCharge = 0d;

        while (feed.HasMoreResults)
        {
            FeedResponse<TfsIntegration> response = await feed.ReadNextAsync(cancellationToken: cancellationToken);

            foreach (TfsIntegration @event in response)
            {
                tfsIntegrationList.Add(@event);
            }

            requestCharge += response.RequestCharge;
        }

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", requestCharge);

        return tfsIntegrationList;
    }

    public async Task<TfsIntegration> CreateTfsIntegration(TfsIntegration tfsIntegration, CancellationToken cancellationToken = default)
    {
        var response = await _container.CreateItemAsync(
            item: tfsIntegration,
            partitionKey: new PartitionKey(tfsIntegration.Id.ToString()),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);

        var tfsIntegrationCreated = response.Resource;

        return tfsIntegrationCreated;
    }

    public async Task<TfsIntegration> GetTfsIntegration(Guid id, CancellationToken cancellationToken = default)
    {
        ItemResponse<TfsIntegration> response = await _container.ReadItemAsync<TfsIntegration>(
                id: id.ToString(),
                partitionKey: new PartitionKey(id.ToString()),
                cancellationToken: cancellationToken
            );

        var tfsIntegration = response.Resource;

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);

        return tfsIntegration;
    }

    public async Task<TfsIntegration> UpdateTfsIntegration(TfsIntegration tfsIntegration, CancellationToken cancellationToken = default)
    {
        var response = await _container.UpsertItemAsync(
            item: tfsIntegration,
            partitionKey: new PartitionKey(tfsIntegration.Id.ToString()),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);

        var tfsIntegrationUpdated = response.Resource;

        return tfsIntegrationUpdated;
    }

    public async Task DeleteTfsIntegration(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _container.DeleteItemAsync<TfsIntegration>(
            id.ToString(),
            new PartitionKey(id.ToString()),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);
    }
}
