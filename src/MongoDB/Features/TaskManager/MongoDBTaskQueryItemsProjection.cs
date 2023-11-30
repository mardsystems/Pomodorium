using MediatR;
using MongoDB.Driver;
using Pomodorium.FlowtimeTechnique.Model;
using Pomodorium.TaskManagement.Model.Integrations;
using Pomodorium.TaskManagement.Model.Tasks;
using System.DomainModel;

namespace Pomodorium.Features.TaskManager;

public class MongoDBTaskQueryItemsProjection :
    IRequestHandler<GetTasksRequest, GetTasksResponse>,
    INotificationHandler<TaskCreated>,
    INotificationHandler<TaskIntegrated>,
    INotificationHandler<TaskDescriptionChanged>,
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
        var filter = Builders<TaskQueryItem>.Filter.Eq(x => x.Id, notification.Id);

        var update = Builders<TaskQueryItem>.Update
            .Set(x => x.Description, notification.Description);

        await _mongoCollection.UpdateManyAsync(filter, update, null, cancellationToken);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeInterrupted notification, CancellationToken cancellationToken)
    {
        var filter = Builders<TaskQueryItem>.Filter.Eq(x => x.Id, notification.Id);

        var taskQueryItem = await _mongoCollection.Find(filter).FirstAsync(cancellationToken);

        if (taskQueryItem == null)
        {
            throw new EntityNotFoundException();
        }

        if (taskQueryItem.TotalHours.HasValue)
        {
            taskQueryItem.TotalHours += notification.Worktime.TotalHours;
        }
        else
        {
            taskQueryItem.TotalHours = notification.Worktime.TotalHours;
        }

        //var update = Builders<TaskQueryItem>.Update
        //    .Set(x => x.StopDateTime, notification.StopDateTime)
        //    .Set(x => x.Interrupted, notification.Interrupted)
        //    .Set(x => x.Worktime, notification.Worktime)
        //    .Set(x => x.Breaktime, notification.Breaktime)
        //    .Set(x => x.State, notification.State)
        //    .Set(x => x.Version, notification.Version);

        //await _mongoCollection.UpdateOneAsync(filter, update, null, cancellationToken);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeStopped notification, CancellationToken cancellationToken)
    {
        var filter = Builders<TaskQueryItem>.Filter.Eq(x => x.Id, notification.Id);

        var taskQueryItem = await _mongoCollection.Find(filter).FirstAsync(cancellationToken);

        if (taskQueryItem == null)
        {
            throw new EntityNotFoundException();
        }

        if (taskQueryItem.TotalHours.HasValue)
        {
            taskQueryItem.TotalHours += notification.Worktime.TotalHours;
        }
        else
        {
            taskQueryItem.TotalHours = notification.Worktime.TotalHours;
        }

        //var update = Builders<TaskQueryItem>.Update
        //    .Set(x => x.StopDateTime, notification.StopDateTime)
        //    .Set(x => x.Interrupted, notification.Interrupted)
        //    .Set(x => x.Worktime, notification.Worktime)
        //    .Set(x => x.Breaktime, notification.Breaktime)
        //    .Set(x => x.State, notification.State)
        //    .Set(x => x.Version, notification.Version);

        //await _mongoCollection.UpdateOneAsync(filter, update, null, cancellationToken);
    }

    public async System.Threading.Tasks.Task Handle(TaskArchived notification, CancellationToken cancellationToken)
    {
        var filter = Builders<TaskQueryItem>.Filter.Eq(x => x.Id, notification.Id);

        var taskQueryItem = await _mongoCollection.Find(filter).FirstAsync(cancellationToken);

        if (taskQueryItem == null)
        {
            throw new EntityNotFoundException();
        }

        await _mongoCollection.DeleteOneAsync(filter, cancellationToken);
    }
}
