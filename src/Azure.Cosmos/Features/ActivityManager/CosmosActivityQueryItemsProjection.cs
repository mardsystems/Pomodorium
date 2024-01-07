using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pomodorium.Data;
using Pomodorium.Models;
using System.DomainModel;
using TaskManagement.Models.Activities;

namespace Pomodorium.Features.ActivityManager;

public class CosmosActivityQueryItemsProjection :
    IRequestHandler<ActivityQueryRequest, ActivityQueryResponse>,
    INotificationHandler<ActivityCreated>,
    INotificationHandler<ActivityUpdated>,
    INotificationHandler<ActivityDeleted>
{
    private readonly CosmosClient _cosmosClient;

    private readonly Container _container;

    private readonly ILogger<CosmosActivityQueryItemsProjection> _logger;

    public CosmosActivityQueryItemsProjection(
        CosmosClient cosmosClient,
        IOptions<CosmosOptions> optionsInterface,
        ILogger<CosmosActivityQueryItemsProjection> logger)
    {
        _cosmosClient = cosmosClient;

        var options = optionsInterface.Value;

        _container = _cosmosClient.GetDatabase(options.Database).GetContainer("ActivityQueryItems");

        _logger = logger;
    }

    public async Task<ActivityQueryResponse> Handle(ActivityQueryRequest request, CancellationToken cancellationToken)
    {
        var activityQueryItems = new List<ActivityQueryItem>();

        var query = new QueryDefinition(
            query: "SELECT * FROM ActivityQueryItems p");

        using var feed = _container.GetItemQueryIterator<ActivityQueryItem>(queryDefinition: query);

        double requestCharge = 0d;

        while (feed.HasMoreResults)
        {
            var feedResponse = await feed.ReadNextAsync(cancellationToken: cancellationToken);

            foreach (ActivityQueryItem item in feedResponse)
            {
                activityQueryItems.Add(item);
            }

            requestCharge += feedResponse.RequestCharge;
        }

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", requestCharge);

        var response = new ActivityQueryResponse(request.GetCorrelationId())
        {
            ActivityQueryItems = activityQueryItems
        };

        return response;
    }

    public async Task Handle(ActivityCreated notification, CancellationToken cancellationToken)
    {
        var activityQueryItem = new ActivityQueryItem
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
            item: activityQueryItem,
            partitionKey: new PartitionKey(notification.ActivityId.ToString()),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);
    }

    public async Task Handle(ActivityUpdated notification, CancellationToken cancellationToken)
    {
        var itemResponse = await _container.ReadItemAsync<ActivityQueryItem>(
                id: notification.ActivityId.ToString(),
                partitionKey: new PartitionKey(notification.ActivityId.ToString()),
                cancellationToken: cancellationToken
            );

        var activityQueryItem = itemResponse.Resource ?? throw new EntityNotFoundException();

        activityQueryItem.Name = notification.ActivityName;
        activityQueryItem.State = notification.ActivityState;
        activityQueryItem.StartDateTime = notification.StartDateTime;
        activityQueryItem.StopDateTime = notification.StopDateTime;
        activityQueryItem.Duration = notification.ActivityDuration;
        activityQueryItem.Description = notification.ActivityDescription;
        activityQueryItem.Version = notification.Version;

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", itemResponse.RequestCharge);

        var response = await _container.UpsertItemAsync(
            item: activityQueryItem,
            partitionKey: new PartitionKey(notification.ActivityId.ToString()),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);
    }

    public async Task Handle(ActivityDeleted notification, CancellationToken cancellationToken)
    {
        var response = await _container.DeleteItemAsync<TfsIntegration>(
            notification.ActivityId.ToString(),
            new PartitionKey(notification.ActivityId.ToString()),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);
    }
}
