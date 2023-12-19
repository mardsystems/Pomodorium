using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pomodorium.Data;
using Pomodorium.Models.PomodoroTechnique;
using System.DomainModel;

namespace Pomodorium.Features.PomodoroTimer;

public class CosmosPomodoroDetailsProjection :
    IRequestHandler<GetPomodoroRequest, GetPomodoroResponse>,
    INotificationHandler<PomodoroCreated>,
    INotificationHandler<PomodoroChecked>,
    INotificationHandler<PomodoroTaskRefined>,
    INotificationHandler<PomodoroArchived>
{
    private readonly CosmosClient _cosmosClient;

    private readonly Container _container;

    private readonly ILogger<CosmosPomodoroDetailsProjection> _logger;

    public CosmosPomodoroDetailsProjection(
        CosmosClient cosmosClient,
        IOptions<CosmosOptions> optionsInterface,
        ILogger<CosmosPomodoroDetailsProjection> logger)
    {
        _cosmosClient = cosmosClient;

        var options = optionsInterface.Value;

        _container = _cosmosClient.GetDatabase(options.Database).GetContainer("PomodoroDetails");

        _logger = logger;
    }

    public async Task<GetPomodoroResponse> Handle(GetPomodoroRequest request, CancellationToken cancellationToken)
    {
        var itemResponse = await _container.ReadItemAsync<PomodoroDetails>(
                id: request.Id.ToString(),
                partitionKey: new PartitionKey(request.Id.ToString()),
                cancellationToken: cancellationToken
            );

        var pomodoroDetails = itemResponse.Resource ?? throw new EntityNotFoundException();

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", itemResponse.RequestCharge);

        var response = new GetPomodoroResponse(request.GetCorrelationId())
        {
            PomodoroDetails = pomodoroDetails
        };

        return response;
    }

    public async Task Handle(PomodoroCreated notification, CancellationToken cancellationToken)
    {
        var pomodoroDetails = new PomodoroDetails
        {
            Id = notification.Id,
            Task = notification.Task,
            Timer = notification.Timer,
            StartDateTime = notification.StartDateTime,
            State = notification.State,
            Version = notification.Version
        };

        var response = await _container.CreateItemAsync(
            item: pomodoroDetails,
            partitionKey: new PartitionKey(notification.Id.ToString()),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);
    }

    public async Task Handle(PomodoroChecked notification, CancellationToken cancellationToken)
    {
        var itemResponse = await _container.ReadItemAsync<PomodoroDetails>(
                id: notification.Id.ToString(),
                partitionKey: new PartitionKey(notification.Id.ToString()),
                cancellationToken: cancellationToken
            );

        var pomodoroDetails = itemResponse.Resource ?? throw new EntityNotFoundException();

        pomodoroDetails.State = notification.State;
        pomodoroDetails.Version = notification.Version;

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", itemResponse.RequestCharge);

        var response = await _container.UpsertItemAsync(
            item: pomodoroDetails,
            partitionKey: new PartitionKey(notification.Id.ToString()),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);
    }

    public async Task Handle(PomodoroTaskRefined notification, CancellationToken cancellationToken)
    {
        var itemResponse = await _container.ReadItemAsync<PomodoroDetails>(
                id: notification.Id.ToString(),
                partitionKey: new PartitionKey(notification.Id.ToString()),
                cancellationToken: cancellationToken
            );

        var pomodoroDetails = itemResponse.Resource ?? throw new EntityNotFoundException();

        pomodoroDetails.Task = notification.Task;
        pomodoroDetails.Version = notification.Version;

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", itemResponse.RequestCharge);

        var response = await _container.UpsertItemAsync(
            item: pomodoroDetails,
            partitionKey: new PartitionKey(notification.Id.ToString()),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);
    }

    public async Task Handle(PomodoroArchived notification, CancellationToken cancellationToken)
    {
        var response = await _container.DeleteItemAsync<PomodoroDetails>(
            notification.Id.ToString(),
            new PartitionKey(notification.Id.ToString()),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Request charge:\t{RequestCharge:0.00}", response.RequestCharge);
    }
}
