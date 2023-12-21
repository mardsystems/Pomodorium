using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pomodorium.Data;
using Pomodorium.Models.FlowtimeTechnique;
using Pomodorium.Models.TaskManagement.Integrations;
using Pomodorium.Models.TaskManagement.Tasks;
using System.DomainModel;

namespace Pomodorium.Features.TaskManager;

public class CosmosTaskQueryItemsProjection :
    IRequestHandler<TaskQueryRequest, TaskQueryResponse>,
    INotificationHandler<TaskCreated>,
    INotificationHandler<TaskIntegrated>,
    INotificationHandler<TaskDescriptionChanged>,
    INotificationHandler<FlowtimeStarted>,
    INotificationHandler<FlowtimeInterrupted>,
    INotificationHandler<FlowtimeStopped>,
    INotificationHandler<TaskArchived>
{
    private readonly CosmosClient _cosmosClient;

    private readonly Container _container;

    private readonly ILogger<CosmosTaskQueryItemsProjection> _logger;

    public CosmosTaskQueryItemsProjection(
        CosmosClient cosmosClient,
        IOptions<CosmosOptions> optionsInterface,
        ILogger<CosmosTaskQueryItemsProjection> logger)
    {
        _cosmosClient = cosmosClient;

        var options = optionsInterface.Value;

        _container = _cosmosClient.GetDatabase(options.Database).GetContainer("TaskQueryItems");

        _logger = logger;
    }

    public async Task<TaskQueryResponse> Handle(TaskQueryRequest request, CancellationToken cancellationToken)
    {
        var taskQueryItems = new List<TaskQueryItem>();

        var query = new QueryDefinition(
            query: @"SELECT * FROM TaskQueryItems p WHERE 1 = 1
AND (IS_NULL(@description) = true OR p.Description = @description)
AND (IS_NULL(@externalReference) = true OR p.ExternalReference = @externalReference)")
            .WithParameter("@description", request.Description)
            .WithParameter("@externalReference", request.ExternalReference);

        using var feed = _container.GetItemQueryIterator<TaskQueryItem>(queryDefinition: query);

        double requestCharge = 0d;

        while (feed.HasMoreResults)
        {
            var feedResponse = await feed.ReadNextAsync(cancellationToken: cancellationToken);

            foreach (TaskQueryItem item in feedResponse)
            {
                taskQueryItems.Add(item);
            }

            requestCharge += feedResponse.RequestCharge;
        }

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", requestCharge);

        var response = new TaskQueryResponse(request.GetCorrelationId())
        {
            TaskQueryItems = taskQueryItems
        };

        return response;
    }

    public async System.Threading.Tasks.Task Handle(TaskCreated notification, CancellationToken cancellationToken)
    {
        var taskQueryItem = new TaskQueryItem
        {
            Id = notification.TaskId,
            CreationDate = notification.TaskCreatedAt,
            Description = notification.TaskDescription,
            Version = notification.Version
        };

        try
        {
            var response = await _container.CreateItemAsync(
                item: taskQueryItem,
                partitionKey: new PartitionKey(notification.TaskId.ToString()),
                cancellationToken: cancellationToken);

            _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error on create task query item. (TaskId: {TaskId})", notification.TaskId);

            throw;
        }
    }

    public async System.Threading.Tasks.Task Handle(TaskIntegrated notification, CancellationToken cancellationToken)
    {
        var itemResponse = await _container.ReadItemAsync<TaskQueryItem>(
                id: notification.TaskId.ToString(),
                partitionKey: new PartitionKey(notification.TaskId.ToString()),
                cancellationToken: cancellationToken
            );

        var taskQueryItem = itemResponse.Resource ?? throw new EntityNotFoundException();

        taskQueryItem.IntegrationType = notification.IntegrationType;
        taskQueryItem.IntegrationId = notification.IntegrationId;
        taskQueryItem.IntegrationName = notification.IntegrationName;
        taskQueryItem.ExternalReference = notification.ExternalReference;

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", itemResponse.RequestCharge);

        var response = await _container.UpsertItemAsync(
            item: taskQueryItem,
            partitionKey: new PartitionKey(notification.TaskId.ToString()),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);
    }

    public async System.Threading.Tasks.Task Handle(TaskDescriptionChanged notification, CancellationToken cancellationToken)
    {
        var itemResponse = await _container.ReadItemAsync<TaskQueryItem>(
                id: notification.TaskId.ToString(),
                partitionKey: new PartitionKey(notification.TaskId.ToString()),
                cancellationToken: cancellationToken
            );

        var taskQueryItem = itemResponse.Resource ?? throw new EntityNotFoundException();

        taskQueryItem.Description = notification.TaskDescription;

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", itemResponse.RequestCharge);

        var response = await _container.UpsertItemAsync(
            item: taskQueryItem,
            partitionKey: new PartitionKey(notification.TaskId.ToString()),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeStarted notification, CancellationToken cancellationToken)
    {
        var itemResponse = await _container.ReadItemAsync<TaskQueryItem>(
                id: notification.TaskId.ToString(),
                partitionKey: new PartitionKey(notification.TaskId.ToString()),
                cancellationToken: cancellationToken
            );

        var taskQueryItem = itemResponse.Resource ?? throw new EntityNotFoundException();

        taskQueryItem.HasFocus = true;

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", itemResponse.RequestCharge);

        var response = await _container.UpsertItemAsync(
            item: taskQueryItem,
            partitionKey: new PartitionKey(notification.TaskId.ToString()),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeInterrupted notification, CancellationToken cancellationToken)
    {
        var itemResponse = await _container.ReadItemAsync<TaskQueryItem>(
                id: notification.TaskId.ToString(),
                partitionKey: new PartitionKey(notification.TaskId.ToString()),
                cancellationToken: cancellationToken
            );

        var taskQueryItem = itemResponse.Resource ?? throw new EntityNotFoundException();

        taskQueryItem.TotalHours += notification.Worktime.TotalHours;

        taskQueryItem.HasFocus = false;

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", itemResponse.RequestCharge);

        var response = await _container.UpsertItemAsync(
            item: taskQueryItem,
            partitionKey: new PartitionKey(notification.TaskId.ToString()),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeStopped notification, CancellationToken cancellationToken)
    {
        var itemResponse = await _container.ReadItemAsync<TaskQueryItem>(
                id: notification.TaskId.ToString(),
                partitionKey: new PartitionKey(notification.TaskId.ToString()),
                cancellationToken: cancellationToken
            );

        var taskQueryItem = itemResponse.Resource ?? throw new EntityNotFoundException();

        taskQueryItem.TotalHours += notification.Worktime.TotalHours;

        taskQueryItem.HasFocus = false;

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", itemResponse.RequestCharge);

        var response = await _container.UpsertItemAsync(
            item: taskQueryItem,
            partitionKey: new PartitionKey(notification.TaskId.ToString()),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);
    }

    public async System.Threading.Tasks.Task Handle(TaskArchived notification, CancellationToken cancellationToken)
    {
        var response = await _container.DeleteItemAsync<TaskQueryItem>(
            notification.TaskId.ToString(),
            new PartitionKey(notification.TaskId.ToString()),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);
    }
}
