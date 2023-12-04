using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pomodorium.Data;
using Pomodorium.Models.FlowtimeTechnique;
using Pomodorium.Models.TaskManagement.Integrations;
using Pomodorium.Models.TaskManagement.Tasks;
using System.DomainModel;

namespace Pomodorium.Features.TaskManager;

public class CosmosTaskQueryItemsProjection :
    IRequestHandler<GetTasksRequest, GetTasksResponse>,
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

    public async Task<GetTasksResponse> Handle(GetTasksRequest request, CancellationToken cancellationToken)
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
            var feedResponse = await feed.ReadNextAsync();

            foreach (TaskQueryItem item in feedResponse)
            {
                taskQueryItems.Add(item);
            }

            requestCharge += feedResponse.RequestCharge;
        }

        _logger.LogInformation($"Request charge:\t{requestCharge:0.00}");

        var response = new GetTasksResponse(request.GetCorrelationId()) { TaskQueryItems = taskQueryItems };

        return response;
    }

    public async System.Threading.Tasks.Task Handle(TaskCreated notification, CancellationToken cancellationToken)
    {
        var taskQueryItem = new TaskQueryItem
        {
            Id = notification.Id,
            CreationDate = notification.CreationDate,
            Description = notification.Description,
            Version = notification.Version
        };

        var response = await _container.CreateItemAsync(item: taskQueryItem, partitionKey: new PartitionKey(notification.Id.ToString()));

        _logger.LogInformation($"Request charge:\t{response.RequestCharge:0.00}");
    }

    public async System.Threading.Tasks.Task Handle(TaskIntegrated notification, CancellationToken cancellationToken)
    {
        var itemResponse = await _container.ReadItemAsync<TaskQueryItem>(
                id: notification.TaskId.ToString(),
                partitionKey: new PartitionKey(notification.TaskId.ToString())
            );

        var taskQueryItem = itemResponse.Resource;

        if (taskQueryItem == null)
        {
            throw new EntityNotFoundException();
        }

        taskQueryItem.IntegrationType = notification.IntegrationType;
        taskQueryItem.IntegrationId = notification.IntegrationId;
        taskQueryItem.IntegrationName = notification.IntegrationName;
        taskQueryItem.ExternalReference = notification.ExternalReference;

        _logger.LogInformation($"Request charge:\t{itemResponse.RequestCharge:0.00}");

        var response = await _container.UpsertItemAsync(item: taskQueryItem, partitionKey: new PartitionKey(notification.TaskId.ToString()));

        _logger.LogInformation($"Request charge:\t{response.RequestCharge:0.00}");
    }

    public async System.Threading.Tasks.Task Handle(TaskDescriptionChanged notification, CancellationToken cancellationToken)
    {
        var itemResponse = await _container.ReadItemAsync<TaskQueryItem>(
                id: notification.Id.ToString(),
                partitionKey: new PartitionKey(notification.Id.ToString())
            );

        var taskQueryItem = itemResponse.Resource;

        if (taskQueryItem == null)
        {
            throw new EntityNotFoundException();
        }

        taskQueryItem.Description = notification.Description;

        _logger.LogInformation($"Request charge:\t{itemResponse.RequestCharge:0.00}");

        var response = await _container.UpsertItemAsync(item: taskQueryItem, partitionKey: new PartitionKey(notification.Id.ToString()));

        _logger.LogInformation($"Request charge:\t{response.RequestCharge:0.00}");
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeStarted notification, CancellationToken cancellationToken)
    {
        var itemResponse = await _container.ReadItemAsync<TaskQueryItem>(
                id: notification.TaskId.ToString(),
                partitionKey: new PartitionKey(notification.TaskId.ToString())
            );

        var taskQueryItem = itemResponse.Resource;

        if (taskQueryItem == null)
        {
            throw new EntityNotFoundException();
        }

        taskQueryItem.HasFocus = true;

        _logger.LogInformation($"Request charge:\t{itemResponse.RequestCharge:0.00}");

        var response = await _container.UpsertItemAsync(item: taskQueryItem, partitionKey: new PartitionKey(notification.TaskId.ToString()));

        _logger.LogInformation($"Request charge:\t{response.RequestCharge:0.00}");
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeInterrupted notification, CancellationToken cancellationToken)
    {
        var itemResponse = await _container.ReadItemAsync<TaskQueryItem>(
                id: notification.TaskId.ToString(),
                partitionKey: new PartitionKey(notification.TaskId.ToString())
            );

        var taskQueryItem = itemResponse.Resource;

        if (taskQueryItem == null)
        {
            throw new EntityNotFoundException();
        }

        taskQueryItem.TotalHours += notification.Worktime.TotalHours;

        taskQueryItem.HasFocus = false;

        _logger.LogInformation($"Request charge:\t{itemResponse.RequestCharge:0.00}");

        var response = await _container.UpsertItemAsync(item: taskQueryItem, partitionKey: new PartitionKey(notification.TaskId.ToString()));

        _logger.LogInformation($"Request charge:\t{response.RequestCharge:0.00}");
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeStopped notification, CancellationToken cancellationToken)
    {
        var itemResponse = await _container.ReadItemAsync<TaskQueryItem>(
                id: notification.TaskId.ToString(),
                partitionKey: new PartitionKey(notification.TaskId.ToString())
            );

        var taskQueryItem = itemResponse.Resource;

        if (taskQueryItem == null)
        {
            throw new EntityNotFoundException();
        }

        taskQueryItem.TotalHours += notification.Worktime.TotalHours;

        taskQueryItem.HasFocus = false;

        _logger.LogInformation($"Request charge:\t{itemResponse.RequestCharge:0.00}");

        var response = await _container.UpsertItemAsync(item: taskQueryItem, partitionKey: new PartitionKey(notification.TaskId.ToString()));

        _logger.LogInformation($"Request charge:\t{response.RequestCharge:0.00}");
    }

    public async System.Threading.Tasks.Task Handle(TaskArchived notification, CancellationToken cancellationToken)
    {
        var response = await _container.DeleteItemAsync<TaskQueryItem>(notification.Id.ToString(), new PartitionKey(notification.Id.ToString()));

        _logger.LogInformation($"Request charge:\t{response.RequestCharge:0.00}");
    }
}
