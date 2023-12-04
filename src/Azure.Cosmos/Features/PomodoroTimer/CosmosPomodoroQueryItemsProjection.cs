using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pomodorium.Data;
using Pomodorium.Models.PomodoroTechnique;
using System.DomainModel;

namespace Pomodorium.Features.PomodoroTimer;

public class CosmosPomodoroQueryItemsProjection :
    IRequestHandler<GetPomosRequest, GetPomosResponse>,
    INotificationHandler<PomodoroCreated>,
    INotificationHandler<PomodoroChecked>,
    INotificationHandler<PomodoroTaskRefined>,
    INotificationHandler<PomodoroArchived>
{
    private readonly CosmosClient _cosmosClient;

    private readonly Container _container;

    private readonly ILogger<CosmosPomodoroQueryItemsProjection> _logger;

    public CosmosPomodoroQueryItemsProjection(
        CosmosClient cosmosClient,
        IOptions<CosmosOptions> optionsInterface,
        ILogger<CosmosPomodoroQueryItemsProjection> logger)
    {
        _cosmosClient = cosmosClient;

        var options = optionsInterface.Value;

        _container = _cosmosClient.GetDatabase(options.Database).GetContainer("PomodoroQueryItems");

        _logger = logger;
    }

    public async Task<GetPomosResponse> Handle(GetPomosRequest request, CancellationToken cancellationToken)
    {
        var pomodoroQueryItems = new List<PomodoroQueryItem>();

        var query = new QueryDefinition(
            query: "SELECT * FROM PomodoroQueryItems p");

        using var feed = _container.GetItemQueryIterator<PomodoroQueryItem>(queryDefinition: query);

        double requestCharge = 0d;

        while (feed.HasMoreResults)
        {
            var feedResponse = await feed.ReadNextAsync();

            foreach (PomodoroQueryItem item in feedResponse)
            {
                pomodoroQueryItems.Add(item);
            }

            requestCharge += feedResponse.RequestCharge;
        }

        _logger.LogInformation($"Request charge:\t{requestCharge:0.00}");

        var response = new GetPomosResponse(request.GetCorrelationId()) { PomodoroQueryItems = pomodoroQueryItems };

        return response;
    }

    public async Task Handle(PomodoroCreated notification, CancellationToken cancellationToken)
    {
        var pomodoroQueryItem = new PomodoroQueryItem
        {
            Id = notification.Id,
            Task = notification.Task,
            Timer = notification.Timer,
            StartDateTime = notification.StartDateTime,
            State = notification.State,
            Version = notification.Version
        };

        var response = await _container.CreateItemAsync(item: pomodoroQueryItem, partitionKey: new PartitionKey(notification.Id.ToString()));

        _logger.LogInformation($"Request charge:\t{response.RequestCharge:0.00}");
    }

    public async Task Handle(PomodoroChecked notification, CancellationToken cancellationToken)
    {
        var itemResponse = await _container.ReadItemAsync<PomodoroQueryItem>(
                id: notification.Id.ToString(),
                partitionKey: new PartitionKey(notification.Id.ToString())
            );

        var pomodoroQueryItem = itemResponse.Resource;

        if (pomodoroQueryItem == null)
        {
            throw new EntityNotFoundException();
        }

        pomodoroQueryItem.State = notification.State;
        pomodoroQueryItem.Version = notification.Version;

        _logger.LogInformation($"Request charge:\t{itemResponse.RequestCharge:0.00}");

        var response = await _container.UpsertItemAsync(item: pomodoroQueryItem, partitionKey: new PartitionKey(notification.Id.ToString()));

        _logger.LogInformation($"Request charge:\t{response.RequestCharge:0.00}");
    }

    public async Task Handle(PomodoroTaskRefined notification, CancellationToken cancellationToken)
    {
        var itemResponse = await _container.ReadItemAsync<PomodoroQueryItem>(
                id: notification.Id.ToString(),
                partitionKey: new PartitionKey(notification.Id.ToString())
            );

        var pomodoroQueryItem = itemResponse.Resource;

        if (pomodoroQueryItem == null)
        {
            throw new EntityNotFoundException();
        }

        pomodoroQueryItem.Task = notification.Task;
        pomodoroQueryItem.Version = notification.Version;

        _logger.LogInformation($"Request charge:\t{itemResponse.RequestCharge:0.00}");

        var response = await _container.UpsertItemAsync(item: pomodoroQueryItem, partitionKey: new PartitionKey(notification.Id.ToString()));

        _logger.LogInformation($"Request charge:\t{response.RequestCharge:0.00}");
    }

    public async Task Handle(PomodoroArchived notification, CancellationToken cancellationToken)
    {
        var response = await _container.DeleteItemAsync<PomodoroQueryItem>(notification.Id.ToString(), new PartitionKey(notification.Id.ToString()));

        _logger.LogInformation($"Request charge:\t{response.RequestCharge:0.00}");
    }
}
