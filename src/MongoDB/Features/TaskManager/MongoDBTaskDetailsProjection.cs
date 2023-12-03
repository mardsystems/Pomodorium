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

        var pomodoroDetails = await _mongoCollection.Find(filter).FirstAsync(cancellationToken);

        if (pomodoroDetails == null)
        {
            throw new EntityNotFoundException();
        }

        var response = new GetTaskResponse(request.GetCorrelationId()) { TaskDetails = pomodoroDetails };

        return response;
    }

    public async System.Threading.Tasks.Task Handle(TaskCreated notification, CancellationToken cancellationToken)
    {
        var flowtimeDetails = new TaskDetails
        {
            Id = notification.Id,
            CreationDate = notification.CreationDate,
            Description = notification.Description,
            Version = notification.Version
        };

        await _mongoCollection.InsertOneAsync(flowtimeDetails, null, cancellationToken);
    }

    public async System.Threading.Tasks.Task Handle(TaskDescriptionChanged notification, CancellationToken cancellationToken)
    {
        var filter = Builders<TaskDetails>.Filter.Eq(x => x.Id, notification.Id);

        var update = Builders<TaskDetails>.Update
            .Set(x => x.Description, notification.Description)
            .Set(x => x.Version, notification.Version);

        await _mongoCollection.UpdateOneAsync(filter, update, null, cancellationToken);
    }

    public async System.Threading.Tasks.Task Handle(TaskArchived notification, CancellationToken cancellationToken)
    {
        var filter = Builders<TaskDetails>.Filter.Eq(x => x.Id, notification.Id);

        await _mongoCollection.DeleteOneAsync(filter, cancellationToken);
    }
}
