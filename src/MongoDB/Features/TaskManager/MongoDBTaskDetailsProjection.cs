using MediatR;
using MongoDB.Driver;
using Pomodorium.Models.TaskManagement.Tasks;
using System.DomainModel;

namespace Pomodorium.Features.TaskManager;

public class MongoDBTaskDetailsProjection :
    IRequestHandler<GetTaskRequest, GetTaskResponse>,
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

    public async Task<GetTaskResponse> Handle(GetTaskRequest request, CancellationToken cancellationToken)
    {
        var filter = Builders<TaskDetails>.Filter.Eq(x => x.Id, request.TaskId);

        var taskDetails = await _mongoCollection.Find(filter).FirstAsync(cancellationToken) ?? throw new EntityNotFoundException();

        var response = new GetTaskResponse(request.GetCorrelationId())
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
