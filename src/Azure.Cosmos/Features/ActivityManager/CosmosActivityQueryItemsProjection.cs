using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pomodorium.Data;
using Pomodorium.Models;
using Pomodorium.Models.TaskManagement.Activities;
using System.DomainModel;

namespace Pomodorium.Features.ActivityManager;

public class CosmosActivityQueryItemsProjection :
    IRequestHandler<GetActivitiesRequest, GetActivitiesResponse>,
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

    public async Task<GetActivitiesResponse> Handle(GetActivitiesRequest request, CancellationToken cancellationToken)
    {
        var activityQueryItems = new List<ActivityQueryItem>();

        var query = new QueryDefinition(
            query: "SELECT * FROM ActivityQueryItems p");

        using var feed = _container.GetItemQueryIterator<ActivityQueryItem>(queryDefinition: query);

        double requestCharge = 0d;

        while (feed.HasMoreResults)
        {
            var feedResponse = await feed.ReadNextAsync();

            foreach (ActivityQueryItem item in feedResponse)
            {
                activityQueryItems.Add(item);
            }

            requestCharge += feedResponse.RequestCharge;
        }

        _logger.LogInformation($"Request charge:\t{requestCharge:0.00}");

        var response = new GetActivitiesResponse(request.GetCorrelationId()) { ActivityQueryItems = activityQueryItems };

        return response;
    }

    public async Task Handle(ActivityCreated notification, CancellationToken cancellationToken)
    {
        var activityQueryItem = new ActivityQueryItem
        {
            Id = notification.Id,
            Name = notification.Name,
            State = notification.State,
            StartDateTime = notification.StartDateTime,
            StopDateTime = notification.StopDateTime,
            Duration = notification.Duration,
            Description = notification.Description,
            Version = notification.Version
        };

        var response = await _container.CreateItemAsync(item: activityQueryItem, partitionKey: new PartitionKey(notification.Id.ToString()));

        _logger.LogInformation($"Request charge:\t{response.RequestCharge:0.00}");
    }

    public async Task Handle(ActivityUpdated notification, CancellationToken cancellationToken)
    {
        var itemResponse = await _container.ReadItemAsync<ActivityQueryItem>(
                id: notification.Id.ToString(),
                partitionKey: new PartitionKey(notification.Id.ToString())
            );

        var activityQueryItem = itemResponse.Resource;

        if (activityQueryItem == null)
        {
            throw new EntityNotFoundException();
        }

        activityQueryItem.Name = notification.Name;
        activityQueryItem.State = notification.State;
        activityQueryItem.StartDateTime = notification.StartDateTime;
        activityQueryItem.StopDateTime = notification.StopDateTime;
        activityQueryItem.Duration = notification.Duration;
        activityQueryItem.Description = notification.Description;
        activityQueryItem.Version = notification.Version;

        _logger.LogInformation($"Request charge:\t{itemResponse.RequestCharge:0.00}");

        var response = await _container.UpsertItemAsync(item: activityQueryItem, partitionKey: new PartitionKey(notification.Id.ToString()));

        _logger.LogInformation($"Request charge:\t{response.RequestCharge:0.00}");
    }

    public async Task Handle(ActivityDeleted notification, CancellationToken cancellationToken)
    {
        var response = await _container.DeleteItemAsync<TfsIntegration>(notification.Id.ToString(), new PartitionKey(notification.Id.ToString()));

        _logger.LogInformation($"Request charge:\t{response.RequestCharge:0.00}");
    }
}
