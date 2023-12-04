using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pomodorium.Data;
using Pomodorium.Models.TaskManagement.Tasks;
using System.DomainModel;

namespace Pomodorium.Features.TaskManager;

public class CosmosTaskDetailsProjection :
    IRequestHandler<GetTaskRequest, GetTaskResponse>,
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

    public async Task<GetTaskResponse> Handle(GetTaskRequest request, CancellationToken cancellationToken)
    {
        var itemResponse = await _container.ReadItemAsync<TaskDetails>(
                id: request.TaskId.ToString(),
                partitionKey: new PartitionKey(request.TaskId.ToString())
            );

        var taskDetails = itemResponse.Resource;

        if (taskDetails == null)
        {
            throw new EntityNotFoundException();
        }

        _logger.LogInformation($"Request charge:\t{itemResponse.RequestCharge:0.00}");

        var response = new GetTaskResponse(request.GetCorrelationId()) { TaskDetails = taskDetails };

        return response;
    }

    public async System.Threading.Tasks.Task Handle(TaskCreated notification, CancellationToken cancellationToken)
    {
        var taskDetails = new TaskDetails
        {
            Id = notification.Id,
            CreationDate = notification.CreationDate,
            Description = notification.Description,
            Version = notification.Version
        };

        var response = await _container.CreateItemAsync(item: taskDetails, partitionKey: new PartitionKey(notification.Id.ToString()));

        _logger.LogInformation($"Request charge:\t{response.RequestCharge:0.00}");
    }

    public async System.Threading.Tasks.Task Handle(TaskDescriptionChanged notification, CancellationToken cancellationToken)
    {
        var itemResponse = await _container.ReadItemAsync<TaskDetails>(
                id: notification.Id.ToString(),
                partitionKey: new PartitionKey(notification.Id.ToString())
            );

        var taskDetails = itemResponse.Resource;

        if (taskDetails == null)
        {
            throw new EntityNotFoundException();
        }

        taskDetails.Description = notification.Description;
        taskDetails.Version = notification.Version;

        _logger.LogInformation($"Request charge:\t{itemResponse.RequestCharge:0.00}");

        var response = await _container.UpsertItemAsync(item: taskDetails, partitionKey: new PartitionKey(notification.Id.ToString()));

        _logger.LogInformation($"Request charge:\t{response.RequestCharge:0.00}");
    }

    public async System.Threading.Tasks.Task Handle(TaskArchived notification, CancellationToken cancellationToken)
    {
        var response = await _container.DeleteItemAsync<TaskDetails>(notification.Id.ToString(), new PartitionKey(notification.Id.ToString()));

        _logger.LogInformation($"Request charge:\t{response.RequestCharge:0.00}");
    }
}
