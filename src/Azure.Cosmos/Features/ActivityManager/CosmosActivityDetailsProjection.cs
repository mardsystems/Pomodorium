using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pomodorium.Data;
using Pomodorium.Models.TaskManagement.Activities;
using System.DomainModel;

namespace Pomodorium.Features.ActivityManager;

public class CosmosActivityDetailsProjection :
    IRequestHandler<GetActivityRequest, GetActivityResponse>,
    INotificationHandler<ActivityCreated>,
    INotificationHandler<ActivityUpdated>,
    INotificationHandler<ActivityDeleted>
{
    private readonly CosmosClient _cosmosClient;

    private readonly Container _container;

    private readonly ILogger<CosmosActivityDetailsProjection> _logger;

    public CosmosActivityDetailsProjection(
        CosmosClient cosmosClient,
        IOptions<CosmosOptions> optionsInterface,
        ILogger<CosmosActivityDetailsProjection> logger)
    {
        _cosmosClient = cosmosClient;

        var options = optionsInterface.Value;

        _container = _cosmosClient.GetDatabase(options.Database).GetContainer("ActivityDetails");

        _logger = logger;
    }

    public async Task<GetActivityResponse> Handle(GetActivityRequest request, CancellationToken cancellationToken)
    {
        var itemResponse = await _container.ReadItemAsync<ActivityDetails>(
                id: request.Id.ToString(),
                partitionKey: new PartitionKey(request.Id.ToString()),
                cancellationToken: cancellationToken
            );

        var activityDetails = itemResponse.Resource ?? throw new EntityNotFoundException();

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", itemResponse.RequestCharge);

        var response = new GetActivityResponse(request.GetCorrelationId())
        {
            ActivityDetails = activityDetails
        };

        return response;
    }

    public async Task Handle(ActivityCreated notification, CancellationToken cancellationToken)
    {
        var activityDetails = new ActivityDetails
        {
            Id = notification.ActivityId,
            Name = notification.ActivityName,
            State = notification.ActivityState,
            StartDateTime = notification.StartDateTime,
            StopDateTime = notification.StopDateTime,
            Duration = notification.ActivityDuration,
            Description = notification.ActivityDescription,
            Version = notification.Version
        };

        var response = await _container.CreateItemAsync(
            item: activityDetails, 
            partitionKey: new PartitionKey(notification.ActivityId.ToString()),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);
    }

    public async Task Handle(ActivityUpdated notification, CancellationToken cancellationToken)
    {
        var itemResponse = await _container.ReadItemAsync<ActivityDetails>(
                id: notification.ActivityId.ToString(),
                partitionKey: new PartitionKey(notification.ActivityId.ToString()),
                cancellationToken: cancellationToken
            );

        var activityDetails = itemResponse.Resource ?? throw new EntityNotFoundException();

        activityDetails.Name = notification.ActivityName;
        activityDetails.State = notification.ActivityState;
        activityDetails.StartDateTime = notification.StartDateTime;
        activityDetails.StopDateTime = notification.StopDateTime;
        activityDetails.Duration = notification.ActivityDuration;
        activityDetails.Description = notification.ActivityDescription;
        activityDetails.Version = notification.Version;

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", itemResponse.RequestCharge);

        var response = await _container.UpsertItemAsync(
            item: activityDetails,
            partitionKey: new PartitionKey(notification.ActivityId.ToString()),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);
    }

    public async Task Handle(ActivityDeleted notification, CancellationToken cancellationToken)
    {
        var response = await _container.DeleteItemAsync<ActivityDetails>(
            notification.ActivityId.ToString(), 
            new PartitionKey(notification.ActivityId.ToString()),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);
    }
}
