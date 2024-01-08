using FlowtimeTechnique.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pomodorium.Data;
using System.DomainModel;
using TaskManagement.Models.Integrations;
using TaskManagement.Models.Tasks;

namespace TaskManagement.Features.TaskManager;

public class CosmosTaskQueryItemsProjection :
    IRequestHandler<TaskQueryRequest, TaskQueryResponse>,
    INotificationHandler<TaskCreated>,
    INotificationHandler<TaskIntegrated>,
    INotificationHandler<TaskDescriptionChanged>,
    INotificationHandler<FlowtimeStarted>,
    INotificationHandler<FlowtimeInterrupted>,
    INotificationHandler<FlowtimeStopped>,
    INotificationHandler<FlowtimeArchived>,
    INotificationHandler<TaskArchived>
{
    private readonly CosmosClient _cosmosClient;

    private readonly Container _container;

    private readonly Repository _repository;

    private readonly ILogger<CosmosTaskQueryItemsProjection> _logger;

    public CosmosTaskQueryItemsProjection(
        CosmosClient cosmosClient,
        IOptions<CosmosOptions> optionsInterface,
        Repository repository,
        ILogger<CosmosTaskQueryItemsProjection> logger)
    {
        _cosmosClient = cosmosClient;

        var options = optionsInterface.Value;

        _container = _cosmosClient.GetDatabase(options.Database).GetContainer("TaskQueryItems");
        
        _repository = repository;

        _logger = logger;
    }

    public async Task<TaskQueryResponse> Handle(TaskQueryRequest request, CancellationToken cancellationToken)
    {
        var taskQueryItems = new List<TaskQueryItem>();

        var query = new QueryDefinition(
            query: @"SELECT * FROM TaskQueryItems p WHERE 1 = 1
AND (IS_NULL(@description) = true OR p.Description LIKE @description)
AND (IS_NULL(@externalReference) = true OR p.ExternalReference = @externalReference)")
            .WithParameter("@description", $"%{request.Description}%")
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
        taskQueryItem.Version = notification.Version;

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
        taskQueryItem.Version = notification.Version;

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
        taskQueryItem.FlowtimeId = notification.FlowtimeId;

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
        taskQueryItem.FlowtimeId = null;

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
        taskQueryItem.FlowtimeId = null;

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", itemResponse.RequestCharge);

        var response = await _container.UpsertItemAsync(
            item: taskQueryItem,
            partitionKey: new PartitionKey(notification.TaskId.ToString()),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeArchived notification, CancellationToken cancellationToken)
    {
        if (notification.TaskId == default)
        {
            try
            {
                var flowtime = await _repository.GetAggregateById<Flowtime>(notification.Id);

                var flowtimeQueryItem = flowtime ?? throw new EntityNotFoundException();

                var itemResponse = await _container.ReadItemAsync<TaskQueryItem>(
                        id: flowtimeQueryItem.TaskId.ToString(),
                        partitionKey: new PartitionKey(flowtimeQueryItem.TaskId.ToString()),
                        cancellationToken: cancellationToken
                    );

                var taskQueryItem = itemResponse.Resource ?? throw new EntityNotFoundException();

                if (flowtimeQueryItem.Worktime != null)
                {
                    taskQueryItem.TotalHours -= flowtimeQueryItem.Worktime.Value.TotalHours;
                }

                _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", itemResponse.RequestCharge);

                var response = await _container.UpsertItemAsync(
                    item: taskQueryItem,
                    partitionKey: new PartitionKey(taskQueryItem.Id.ToString()),
                    cancellationToken: cancellationToken);

                _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);
            }
            catch (Exception)
            {
                // Do nothing.
            }
        }
        else
        {
            var itemResponse = await _container.ReadItemAsync<TaskQueryItem>(
                    id: notification.TaskId.ToString(),
                    partitionKey: new PartitionKey(notification.TaskId.ToString()),
                    cancellationToken: cancellationToken
                );

            var taskQueryItem = itemResponse.Resource ?? throw new EntityNotFoundException();

            if (notification.Worktime != null)
            {
                taskQueryItem.TotalHours -= notification.Worktime.Value.TotalHours;
            }

            _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", itemResponse.RequestCharge);

            var response = await _container.UpsertItemAsync(
                item: taskQueryItem,
                partitionKey: new PartitionKey(notification.TaskId.ToString()),
                cancellationToken: cancellationToken);

            _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);
        }
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
