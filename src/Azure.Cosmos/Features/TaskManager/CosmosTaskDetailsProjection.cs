using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pomodorium.Data;
using System.DomainModel;
using TaskManagement.Models.Tasks;

namespace TaskManagement.Features.TaskManager;

public class CosmosTaskDetailsProjection :
    IRequestHandler<TaskDetailsRequest, TaskDetailsResponse>,
    INotificationHandler<TaskCreated>,
    INotificationHandler<TaskDescriptionChanged>,
    INotificationHandler<TaskArchived>
{
    private readonly CosmosClient _cosmosClient;

    private readonly Container _container;

    private readonly ILogger<CosmosTaskDetailsProjection> _logger;

    public CosmosTaskDetailsProjection(
        CosmosClient cosmosClient,
        IOptions<CosmosOptions> optionsInterface,
        ILogger<CosmosTaskDetailsProjection> logger)
    {
        _cosmosClient = cosmosClient;

        var options = optionsInterface.Value;

        _container = _cosmosClient.GetDatabase(options.Database).GetContainer("TaskDetails");

        _logger = logger;
    }

    public async Task<TaskDetailsResponse> Handle(TaskDetailsRequest request, CancellationToken cancellationToken)
    {
        var itemResponse = await _container.ReadItemAsync<TaskDetails>(
                id: request.TaskId.ToString(),
                partitionKey: new PartitionKey(request.TaskId.ToString()),
                cancellationToken: cancellationToken
            );

        var taskDetails = itemResponse.Resource ?? throw new EntityNotFoundException();

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", itemResponse.RequestCharge);

        var response = new TaskDetailsResponse(request.GetCorrelationId())
        {
            TaskDetails = taskDetails
        };

        return response;
    }

    public async System.Threading.Tasks.Task Handle(TaskCreated notification, CancellationToken cancellationToken)
    {
        var taskDetails = new TaskDetails
        {
            Id = notification.TaskId,
            CreationDate = notification.TaskCreatedAt,
            Description = notification.TaskDescription,
            Version = notification.Version
        };

        try
        {
            var response = await _container.CreateItemAsync(
                item: taskDetails,
                partitionKey: new PartitionKey(notification.TaskId.ToString()),
                cancellationToken: cancellationToken);

            _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error on create task details. (TaskId: {TaskId})", notification.TaskId);

            throw;
        }
    }

    public async System.Threading.Tasks.Task Handle(TaskDescriptionChanged notification, CancellationToken cancellationToken)
    {
        var itemResponse = await _container.ReadItemAsync<TaskDetails>(
                id: notification.TaskId.ToString(),
                partitionKey: new PartitionKey(notification.TaskId.ToString()),
                cancellationToken: cancellationToken
            );

        var taskDetails = itemResponse.Resource ?? throw new EntityNotFoundException();

        taskDetails.Description = notification.TaskDescription;
        taskDetails.Version = notification.Version;

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", itemResponse.RequestCharge);

        var response = await _container.UpsertItemAsync(
            item: taskDetails,
            partitionKey: new PartitionKey(notification.TaskId.ToString()),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);
    }

    public async System.Threading.Tasks.Task Handle(TaskArchived notification, CancellationToken cancellationToken)
    {
        var response = await _container.DeleteItemAsync<TaskDetails>(
            notification.TaskId.ToString(),
            new PartitionKey(notification.TaskId.ToString()),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);
    }
}
