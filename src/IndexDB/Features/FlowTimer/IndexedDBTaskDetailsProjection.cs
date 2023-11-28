using MediatR;
using Pomodorium.Data;
using Pomodorium.Features.TaskManager;
using Pomodorium.TaskManagement.Model.Tasks;
using System.DomainModel;

namespace Pomodorium.Features.FlowTimer;

public class IndexedDBTaskDetailsProjection :
    IRequestHandler<GetTaskRequest, GetTaskResponse>,
    INotificationHandler<TaskCreated>,
    INotificationHandler<TaskDescriptionChanged>,
    INotificationHandler<TaskArchived>
{
    private readonly IndexedDBAccessor _db;

    public IndexedDBTaskDetailsProjection(IndexedDBAccessor db)
    {
        _db = db;
    }

    public async Task<GetTaskResponse> Handle(GetTaskRequest request, CancellationToken cancellationToken)
    {
        var taskDetails = await _db.GetAsync<TaskDetails>("TaskDetails", request.Id);

        if (taskDetails == null)
        {
            throw new EntityNotFoundException();
        }

        var response = new GetTaskResponse(request.GetCorrelationId()) { TaskDetails = taskDetails };

        return response;
    }

    public async System.Threading.Tasks.Task Handle(TaskCreated notification, CancellationToken cancellationToken)
    {
        var taskDetails = new TaskDetails
        {
            Id = notification.Id,
            CreationDate = notification.CreationDate,
            Description = notification.Description,
            Version = notification.Version
        };

        await _db.PutAsync("TaskDetails", taskDetails);
    }

    public async System.Threading.Tasks.Task Handle(TaskDescriptionChanged notification, CancellationToken cancellationToken)
    {
        var taskDetails = await _db.GetAsync<TaskDetails>("TaskDetails", notification.Id);

        if (taskDetails == null)
        {
            throw new EntityNotFoundException();
        }

        taskDetails.Description = notification.Description;
        taskDetails.Version = notification.Version;

        await _db.PutAsync("TaskDetails", taskDetails);
    }

    public async System.Threading.Tasks.Task Handle(TaskArchived notification, CancellationToken cancellationToken)
    {
        var taskDetails = await _db.GetAsync<TaskDetails>("TaskDetails", notification.Id);

        if (taskDetails == null)
        {
            throw new EntityNotFoundException();
        }

        await _db.RemoveAsync("TaskDetails", notification.Id);
    }
}
