﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pomodorium.Data;
using Pomodorium.Models.FlowtimeTechnique;
using Pomodorium.Models.TaskManagement.Tasks;
using System.DomainModel;

namespace Pomodorium.Features.FlowTimer;

public class CosmosFlowtimeDetailsProjection :
    IRequestHandler<GetFlowtimeRequest, GetFlowtimeResponse>,
    INotificationHandler<FlowtimeCreated>,
    INotificationHandler<FlowtimeStarted>,
    INotificationHandler<FlowtimeInterrupted>,
    INotificationHandler<FlowtimeStopped>,
    INotificationHandler<TaskDescriptionChanged>,
    INotificationHandler<FlowtimeArchived>
{
    private readonly CosmosClient _cosmosClient;

    private readonly Container _container;

    private readonly ILogger<CosmosFlowtimeDetailsProjection> _logger;

    public CosmosFlowtimeDetailsProjection(
        CosmosClient cosmosClient,
        IOptions<CosmosOptions> optionsInterface,
        ILogger<CosmosFlowtimeDetailsProjection> logger)
    {
        _cosmosClient = cosmosClient;

        var options = optionsInterface.Value;

        _container = _cosmosClient.GetDatabase(options.Database).GetContainer("FlowtimeDetails");

        _logger = logger;
    }

    public async Task<GetFlowtimeResponse> Handle(GetFlowtimeRequest request, CancellationToken cancellationToken)
    {
        var itemResponse = await _container.ReadItemAsync<FlowtimeDetails>(
                id: request.Id.ToString(),
                partitionKey: new PartitionKey(request.Id.ToString())
            );

        var flowtimeDetails = itemResponse.Resource;

        if (flowtimeDetails == null)
        {
            throw new EntityNotFoundException();
        }

        _logger.LogInformation($"Request charge:\t{itemResponse.RequestCharge:0.00}");

        var response = new GetFlowtimeResponse(request.GetCorrelationId()) { FlowtimeDetails = flowtimeDetails };

        return response;
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeCreated notification, CancellationToken cancellationToken)
    {
        var flowtimeDetails = new FlowtimeDetails
        {
            Id = notification.Id,
            CreationDate = notification.CreationDate,
            State = notification.State,
            TaskId = notification.TaskId,
            TaskDescription = notification.TaskDescription,
            TaskVersion = notification.TaskVersion,
            Version = notification.Version
        };

        var response = await _container.CreateItemAsync(item: flowtimeDetails, partitionKey: new PartitionKey(notification.Id.ToString()));

        _logger.LogInformation($"Request charge:\t{response.RequestCharge:0.00}");
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeStarted notification, CancellationToken cancellationToken)
    {
        var itemResponse = await _container.ReadItemAsync<FlowtimeDetails>(
                id: notification.Id.ToString(),
                partitionKey: new PartitionKey(notification.Id.ToString())
            );

        var flowtimeDetails = itemResponse.Resource;

        if (flowtimeDetails == null)
        {
            throw new EntityNotFoundException();
        }

        flowtimeDetails.StartDateTime = notification.StartDateTime;
        flowtimeDetails.State = notification.State;
        flowtimeDetails.Version = notification.Version;

        _logger.LogInformation($"Request charge:\t{itemResponse.RequestCharge:0.00}");

        var response = await _container.UpsertItemAsync(item: flowtimeDetails, partitionKey: new PartitionKey(notification.Id.ToString()));

        _logger.LogInformation($"Request charge:\t{response.RequestCharge:0.00}");
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeInterrupted notification, CancellationToken cancellationToken)
    {
        var itemResponse = await _container.ReadItemAsync<FlowtimeDetails>(
                id: notification.Id.ToString(),
                partitionKey: new PartitionKey(notification.Id.ToString())
            );

        var flowtimeDetails = itemResponse.Resource;

        if (flowtimeDetails == null)
        {
            throw new EntityNotFoundException();
        }

        flowtimeDetails.StopDateTime = notification.StopDateTime;
        flowtimeDetails.Interrupted = notification.Interrupted;
        flowtimeDetails.Worktime = notification.Worktime;
        flowtimeDetails.Breaktime = notification.Breaktime;
        flowtimeDetails.State = notification.State;
        flowtimeDetails.Version = notification.Version;

        _logger.LogInformation($"Request charge:\t{itemResponse.RequestCharge:0.00}");

        var response = await _container.UpsertItemAsync(item: flowtimeDetails, partitionKey: new PartitionKey(notification.Id.ToString()));

        _logger.LogInformation($"Request charge:\t{response.RequestCharge:0.00}");
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeStopped notification, CancellationToken cancellationToken)
    {
        var itemResponse = await _container.ReadItemAsync<FlowtimeDetails>(
                id: notification.Id.ToString(),
                partitionKey: new PartitionKey(notification.Id.ToString())
            );

        var flowtimeDetails = itemResponse.Resource;

        if (flowtimeDetails == null)
        {
            throw new EntityNotFoundException();
        }

        flowtimeDetails.StopDateTime = notification.StopDateTime;
        flowtimeDetails.Interrupted = notification.Interrupted;
        flowtimeDetails.Worktime = notification.Worktime;
        flowtimeDetails.Breaktime = notification.Breaktime;
        flowtimeDetails.State = notification.State;
        flowtimeDetails.Version = notification.Version;

        _logger.LogInformation($"Request charge:\t{itemResponse.RequestCharge:0.00}");

        var response = await _container.UpsertItemAsync(item: flowtimeDetails, partitionKey: new PartitionKey(notification.Id.ToString()));

        _logger.LogInformation($"Request charge:\t{response.RequestCharge:0.00}");
    }

    public async System.Threading.Tasks.Task Handle(TaskDescriptionChanged notification, CancellationToken cancellationToken)
    {
        var flowtimeDetailsByTaskId = new List<FlowtimeDetails>();

        var query = new QueryDefinition(
            query: "SELECT * FROM FlowtimeDetails p WHERE p.TaskId = @taskId")
            .WithParameter("@taskId", notification.Id);

        using var feed = _container.GetItemQueryIterator<FlowtimeDetails>(queryDefinition: query);

        double requestCharge = 0d;

        while (feed.HasMoreResults)
        {
            var feedResponse = await feed.ReadNextAsync();

            foreach (FlowtimeDetails item in feedResponse)
            {
                flowtimeDetailsByTaskId.Add(item);
            }

            requestCharge += feedResponse.RequestCharge;
        }

        _logger.LogInformation($"Request charge:\t{requestCharge:0.00}");

        foreach (var flowtimeDetails in flowtimeDetailsByTaskId)
        {
            flowtimeDetails.TaskDescription = notification.Description;
            flowtimeDetails.TaskVersion = notification.Version;

            var response = await _container.UpsertItemAsync(item: flowtimeDetails, partitionKey: new PartitionKey(notification.Id.ToString()));

            _logger.LogInformation($"Request charge:\t{response.RequestCharge:0.00}");
        }
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeArchived notification, CancellationToken cancellationToken)
    {
        var response = await _container.DeleteItemAsync<FlowtimeDetails>(notification.Id.ToString(), new PartitionKey(notification.Id.ToString()));

        _logger.LogInformation($"Request charge:\t{response.RequestCharge:0.00}");
    }
}