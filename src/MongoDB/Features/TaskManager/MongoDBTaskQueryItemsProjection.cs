using MediatR;
using MongoDB.Driver;
using Pomodorium.Models.FlowtimeTechnique;
using Pomodorium.Models.TaskManagement.Integrations;
using Pomodorium.Models.TaskManagement.Tasks;

namespace Pomodorium.Features.TaskManager;

public class MongoDBTaskQueryItemsProjection :
    IRequestHandler<GetTasksRequest, GetTasksResponse>,
    INotificationHandler<TaskCreated>,
    INotificationHandler<TaskIntegrated>,
    INotificationHandler<TaskDescriptionChanged>,
    INotificationHandler<FlowtimeStarted>,
    INotificationHandler<FlowtimeInterrupted>,
    INotificationHandler<FlowtimeStopped>,
    INotificationHandler<TaskArchived>
{
    private readonly MongoClient _mongoClient;

    private readonly IMongoCollection<TaskQueryItem> _mongoCollection;

    public MongoDBTaskQueryItemsProjection(MongoClient mongoClient)
    {
        _mongoClient = mongoClient;

        _mongoCollection = _mongoClient.GetDatabase("Pomodorium").GetCollection<TaskQueryItem>("TaskQueryItems");
    }

    public async Task<GetTasksResponse> Handle(GetTasksRequest request, CancellationToken cancellationToken)
    {
        FilterDefinition<TaskQueryItem> filter;

        if (request.ExternalReference == null)
        {
            filter = Builders<TaskQueryItem>.Filter.Empty;
        }
        else
        {
            filter = Builders<TaskQueryItem>.Filter.Eq(x => x.ExternalReference, request.ExternalReference);
        }

        var taskQueryItems = await _mongoCollection.Find(filter).ToListAsync(cancellationToken);

        var response = new GetTasksResponse(request.GetCorrelationId())
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

        await _mongoCollection.InsertOneAsync(taskQueryItem, null, cancellationToken);
    }

    public async System.Threading.Tasks.Task Handle(TaskIntegrated notification, CancellationToken cancellationToken)
    {
        var filter = Builders<TaskQueryItem>.Filter.Eq(x => x.Id, notification.TaskId);

        var update = Builders<TaskQueryItem>.Update
            .Set(x => x.IntegrationType, notification.IntegrationType)
            .Set(x => x.IntegrationId, notification.IntegrationId)
            .Set(x => x.IntegrationName, notification.IntegrationName)
            .Set(x => x.ExternalReference, notification.ExternalReference);

        await _mongoCollection.UpdateManyAsync(filter, update, null, cancellationToken);
    }

    public async System.Threading.Tasks.Task Handle(TaskDescriptionChanged notification, CancellationToken cancellationToken)
    {
        var filter = Builders<TaskQueryItem>.Filter.Eq(x => x.Id, notification.TaskId);

        var update = Builders<TaskQueryItem>.Update
            .Set(x => x.Description, notification.TaskDescription);

        await _mongoCollection.UpdateManyAsync(filter, update, null, cancellationToken);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeStarted notification, CancellationToken cancellationToken)
    {
        var filter = Builders<TaskQueryItem>.Filter.Eq(x => x.Id, notification.TaskId);

        var update = Builders<TaskQueryItem>.Update
            .Set(x => x.HasFocus, true);

        await _mongoCollection.UpdateManyAsync(filter, update, null, cancellationToken);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeInterrupted notification, CancellationToken cancellationToken)
    {
        var filter = Builders<TaskQueryItem>.Filter.Eq(x => x.Id, notification.TaskId);

        var update = Builders<TaskQueryItem>.Update
            .Inc(x => x.TotalHours, notification.Worktime.TotalHours)
            .Set(x => x.HasFocus, false);

        await _mongoCollection.UpdateManyAsync(filter, update, null, cancellationToken);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeStopped notification, CancellationToken cancellationToken)
    {
        var filter = Builders<TaskQueryItem>.Filter.Eq(x => x.Id, notification.TaskId);

        var update = Builders<TaskQueryItem>.Update
            .Inc(x => x.TotalHours, notification.Worktime.TotalHours)
            .Set(x => x.HasFocus, false);

        await _mongoCollection.UpdateManyAsync(filter, update, null, cancellationToken);
    }

    public async System.Threading.Tasks.Task Handle(TaskArchived notification, CancellationToken cancellationToken)
    {
        var filter = Builders<TaskQueryItem>.Filter.Eq(x => x.Id, notification.TaskId);

        await _mongoCollection.DeleteOneAsync(filter, cancellationToken);
    }
}
