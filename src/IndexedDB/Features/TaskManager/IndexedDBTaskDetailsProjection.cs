using MediatR;
using Pomodorium.Data;
using System.DomainModel;
using TaskManagement.Models.Tasks;

namespace Pomodorium.Features.TaskManager;

public class IndexedDBTaskDetailsProjection :
    IRequestHandler<TaskDetailsRequest, TaskDetailsResponse>,
    INotificationHandler<TaskCreated>,
    INotificationHandler<TaskDescriptionChanged>,
    INotificationHandler<TaskArchived>
{
    private readonly IndexedDBAccessor _db;

    public IndexedDBTaskDetailsProjection(IndexedDBAccessor db)
    {
        _db = db;
    }

    public async Task<TaskDetailsResponse> Handle(TaskDetailsRequest request, CancellationToken cancellationToken)
    {
        var taskDetails = await _db.GetAsync<TaskDetails>("TaskDetails", request.TaskId) ?? throw new EntityNotFoundException();

        var response = new TaskDetailsResponse(request.GetCorrelationId())
        {
            TaskDetails = taskDetails
        };

        return response;
    }

    public async System.Threading.Tasks.Task Handle(TaskCreated notification, CancellationToken cancellationToken)
    {
        var taskDetails = new TaskDetails
        {
            Id = notification.TaskId,
            CreationDate = notification.TaskCreatedAt,
            Description = notification.TaskDescription,
            Version = notification.Version
        };

        await _db.PutAsync("TaskDetails", taskDetails);
    }

    public async System.Threading.Tasks.Task Handle(TaskDescriptionChanged notification, CancellationToken cancellationToken)
    {
        var taskDetails = await _db.GetAsync<TaskDetails>("TaskDetails", notification.TaskId) ?? throw new EntityNotFoundException();

        taskDetails.Description = notification.TaskDescription;
        taskDetails.Version = notification.Version;

        await _db.PutAsync("TaskDetails", taskDetails);
    }

    public async System.Threading.Tasks.Task Handle(TaskArchived notification, CancellationToken cancellationToken)
    {
        var _ = await _db.GetAsync<TaskDetails>("TaskDetails", notification.TaskId) ?? throw new EntityNotFoundException();

        await _db.RemoveAsync("TaskDetails", notification.TaskId);
    }
}
