using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pomodorium.Data;
using Pomodorium.Models.FlowtimeTechnique;
using Pomodorium.Models.TaskManagement.Tasks;
using System.DomainModel;

namespace Pomodorium.Features.FlowTimer;

public class CosmosFlowtimeQueryItemsProjection :
    IRequestHandler<FlowtimeQueryRequest, FlowtimeQueryResponse>,
    INotificationHandler<FlowtimeCreated>,
    INotificationHandler<FlowtimeStarted>,
    INotificationHandler<FlowtimeInterrupted>,
    INotificationHandler<FlowtimeStopped>,
    INotificationHandler<TaskDescriptionChanged>,
    INotificationHandler<FlowtimeArchived>
{
    private readonly CosmosClient _cosmosClient;

    private readonly Container _container;

    private readonly ILogger<CosmosFlowtimeQueryItemsProjection> _logger;

    public CosmosFlowtimeQueryItemsProjection(
        CosmosClient cosmosClient,
        IOptions<CosmosOptions> optionsInterface,
        ILogger<CosmosFlowtimeQueryItemsProjection> logger)
    {
        _cosmosClient = cosmosClient;

        var options = optionsInterface.Value;

        _container = _cosmosClient.GetDatabase(options.Database).GetContainer("FlowtimeQueryItems");

        _logger = logger;
    }

    public async Task<FlowtimeQueryResponse> Handle(FlowtimeQueryRequest request, CancellationToken cancellationToken)
    {
        var flowtimeQueryItems = new List<FlowtimeQueryItem>();

        var query = new QueryDefinition(
            query: "SELECT * FROM FlowtimeQueryItems p");

        using var feed = _container.GetItemQueryIterator<FlowtimeQueryItem>(queryDefinition: query);

        double requestCharge = 0d;

        while (feed.HasMoreResults)
        {
            var feedResponse = await feed.ReadNextAsync(cancellationToken: cancellationToken);

            foreach (FlowtimeQueryItem item in feedResponse)
            {
                flowtimeQueryItems.Add(item);
            }

            requestCharge += feedResponse.RequestCharge;
        }

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", requestCharge);

        var response = new FlowtimeQueryResponse(request.GetCorrelationId()) { FlowtimeQueryItems = flowtimeQueryItems };

        return response;
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeCreated notification, CancellationToken cancellationToken)
    {
        var flowtimeQueryItem = new FlowtimeQueryItem
        {
            Id = notification.Id,
            CreationDate = notification.CreationDate,
            State = notification.State,
            TaskId = notification.TaskId,
            TaskDescription = notification.TaskDescription,
            Version = notification.Version
        };

        var response = await _container.CreateItemAsync(
            item: flowtimeQueryItem,
            partitionKey: new PartitionKey(notification.Id.ToString()),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeStarted notification, CancellationToken cancellationToken)
    {
        var itemResponse = await _container.ReadItemAsync<FlowtimeQueryItem>(
                id: notification.FlowtimeId.ToString(),
                partitionKey: new PartitionKey(notification.FlowtimeId.ToString()),
                cancellationToken: cancellationToken
            );

        var flowtimeQueryItem = itemResponse.Resource ?? throw new EntityNotFoundException();

        flowtimeQueryItem.StartDateTime = notification.StartedAt;
        flowtimeQueryItem.State = notification.FlowtimeState;
        flowtimeQueryItem.Version = notification.Version;

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", itemResponse.RequestCharge);

        var response = await _container.UpsertItemAsync(
            item: flowtimeQueryItem,
            partitionKey: new PartitionKey(notification.FlowtimeId.ToString()),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeInterrupted notification, CancellationToken cancellationToken)
    {
        var itemResponse = await _container.ReadItemAsync<FlowtimeQueryItem>(
                id: notification.Id.ToString(),
                partitionKey: new PartitionKey(notification.Id.ToString()),
                cancellationToken: cancellationToken
            );

        var flowtimeQueryItem = itemResponse.Resource ?? throw new EntityNotFoundException();

        flowtimeQueryItem.StopDateTime = notification.StopDateTime;
        flowtimeQueryItem.Interrupted = notification.Interrupted;
        flowtimeQueryItem.Worktime = notification.Worktime;
        flowtimeQueryItem.Breaktime = notification.Breaktime;
        flowtimeQueryItem.State = notification.State;
        flowtimeQueryItem.Version = notification.Version;

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", itemResponse.RequestCharge);

        var response = await _container.UpsertItemAsync(
            item: flowtimeQueryItem,
            partitionKey: new PartitionKey(notification.Id.ToString()),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeStopped notification, CancellationToken cancellationToken)
    {
        var itemResponse = await _container.ReadItemAsync<FlowtimeQueryItem>(
                id: notification.Id.ToString(),
                partitionKey: new PartitionKey(notification.Id.ToString()),
                cancellationToken: cancellationToken
            );

        var flowtimeQueryItem = itemResponse.Resource ?? throw new EntityNotFoundException();

        flowtimeQueryItem.StopDateTime = notification.StopDateTime;
        flowtimeQueryItem.Interrupted = notification.Interrupted;
        flowtimeQueryItem.Worktime = notification.Worktime;
        flowtimeQueryItem.Breaktime = notification.Breaktime;
        flowtimeQueryItem.State = notification.State;
        flowtimeQueryItem.Version = notification.Version;

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", itemResponse.RequestCharge);

        var response = await _container.UpsertItemAsync(
            item: flowtimeQueryItem,
            partitionKey: new PartitionKey(notification.Id.ToString()),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);
    }

    public async System.Threading.Tasks.Task Handle(TaskDescriptionChanged notification, CancellationToken cancellationToken)
    {
        var flowtimeQueryItemsByTaskId = new List<FlowtimeQueryItem>();

        var query = new QueryDefinition(
            query: "SELECT * FROM FlowtimeQueryItems p WHERE p.TaskId = @taskId")
            .WithParameter("@taskId", notification.TaskId);

        using var feed = _container.GetItemQueryIterator<FlowtimeQueryItem>(queryDefinition: query);

        double requestCharge = 0d;

        while (feed.HasMoreResults)
        {
            var feedResponse = await feed.ReadNextAsync(cancellationToken: cancellationToken);

            foreach (FlowtimeQueryItem item in feedResponse)
            {
                flowtimeQueryItemsByTaskId.Add(item);
            }

            requestCharge += feedResponse.RequestCharge;
        }

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", requestCharge);

        foreach (var flowtimeQueryItem in flowtimeQueryItemsByTaskId)
        {
            flowtimeQueryItem.TaskDescription = notification.TaskDescription;

            var response = await _container.UpsertItemAsync(
                item: flowtimeQueryItem,
                partitionKey: new PartitionKey(flowtimeQueryItem.Id.ToString()),
                cancellationToken: cancellationToken);

            _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);
        }
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeArchived notification, CancellationToken cancellationToken)
    {
        var response = await _container.DeleteItemAsync<FlowtimeQueryItem>(
            notification.Id.ToString(),
            new PartitionKey(notification.Id.ToString()),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);
    }
}
