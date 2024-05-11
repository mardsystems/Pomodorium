using MediatR;
using MongoDB.Driver;
using System.DomainModel;
using Pomodorium.Features.TaskManager;
using Pomodorium.Models.Tasks;

namespace Pomodorium.Data;

public class MongoDBTaskDetailsProjection :
    IRequestHandler<TaskDetailsRequest, TaskDetailsResponse>,
    INotificationHandler<TaskCreated>,
    INotificationHandler<TaskDescriptionChanged>,
    INotificationHandler<TaskArchived>
{
    private readonly MongoClient _mongoClient;

    private readonly IMongoCollection<TaskDetails> _mongoCollection;

    public MongoDBTaskDetailsProjection(MongoClient mongoClient)
    {
        _mongoClient = mongoClient;

        _mongoCollection = _mongoClient.GetDatabase("Pomodorium").GetCollection<TaskDetails>("TaskDetails");
    }

    public async Task<TaskDetailsResponse> Handle(TaskDetailsRequest request, CancellationToken cancellationToken)
    {
        var filter = Builders<TaskDetails>.Filter.Eq(x => x.Id, request.TaskId);

        var taskDetails = await _mongoCollection.Find(filter).FirstAsync(cancellationToken) ?? throw new EntityNotFoundException();

        var response = new TaskDetailsResponse(request.GetCorrelationId())
        {
            TaskDetails = taskDetails
        };

        return response;
    }

    public async System.Threading.Tasks.Task Handle(TaskCreated notification, CancellationToken cancellationToken)
    {
        var flowtimeDetails = new TaskDetails
        {
            Id = notification.TaskId,
            CreationDate = notification.TaskCreatedAt,
            Description = notification.TaskDescription,
            Version = notification.Version
        };

        await _mongoCollection.InsertOneAsync(flowtimeDetails, null, cancellationToken);
    }

    public async System.Threading.Tasks.Task Handle(TaskDescriptionChanged notification, CancellationToken cancellationToken)
    {
        var filter = Builders<TaskDetails>.Filter.Eq(x => x.Id, notification.TaskId);

        var update = Builders<TaskDetails>.Update
            .Set(x => x.Description, notification.TaskDescription)
            .Set(x => x.Version, notification.Version);

        await _mongoCollection.UpdateOneAsync(filter, update, null, cancellationToken);
    }

    public async System.Threading.Tasks.Task Handle(TaskArchived notification, CancellationToken cancellationToken)
    {
        var filter = Builders<TaskDetails>.Filter.Eq(x => x.Id, notification.TaskId);

        await _mongoCollection.DeleteOneAsync(filter, cancellationToken);
    }
}
