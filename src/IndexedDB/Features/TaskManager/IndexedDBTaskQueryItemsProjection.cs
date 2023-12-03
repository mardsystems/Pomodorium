using MediatR;
using Pomodorium.Data;
using Pomodorium.Models.FlowtimeTechnique;
using Pomodorium.Models.TaskManagement.Integrations;
using Pomodorium.Models.TaskManagement.Tasks;
using System.DomainModel;

namespace Pomodorium.Features.TaskManager;

public class IndexedDBTaskQueryItemsProjection :
    IRequestHandler<GetTasksRequest, GetTasksResponse>,
    INotificationHandler<TaskCreated>,
    INotificationHandler<TaskIntegrated>,
    INotificationHandler<TaskDescriptionChanged>,
    INotificationHandler<FlowtimeStarted>,
    INotificationHandler<FlowtimeInterrupted>,
    INotificationHandler<FlowtimeStopped>,
    INotificationHandler<TaskArchived>
{
    private readonly IndexedDBAccessor _db;

    public IndexedDBTaskQueryItemsProjection(IndexedDBAccessor db)
    {
        _db = db;
    }

    public async Task<GetTasksResponse> Handle(GetTasksRequest request, CancellationToken cancellationToken)
    {
        var taskQueryItems = await _db.GetAllAsync<TaskQueryItem>("TaskQueryItems");

        var response = new GetTasksResponse(request.GetCorrelationId()) { TaskQueryItems = taskQueryItems.OrderByDescending(x => x.CreationDate) };

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

        await _db.PutAsync("TaskQueryItems", taskQueryItem);
    }

    public async System.Threading.Tasks.Task Handle(TaskIntegrated notification, CancellationToken cancellationToken)
    {
        var taskQueryItem = await _db.GetAsync<TaskQueryItem>("TaskQueryItems", notification.TaskId);

        taskQueryItem.IntegrationType = notification.IntegrationType;
        taskQueryItem.IntegrationId = notification.IntegrationId;
        taskQueryItem.IntegrationName = notification.IntegrationName;
        taskQueryItem.ExternalReference = notification.ExternalReference;

        await _db.PutAsync("TaskQueryItems", taskQueryItem);
    }

    public async System.Threading.Tasks.Task Handle(TaskDescriptionChanged notification, CancellationToken cancellationToken)
    {
        var taskQueryItem = await _db.GetAsync<TaskQueryItem>("TaskQueryItems", notification.Id);

        taskQueryItem.Description = notification.Description;

        await _db.PutAsync("TaskQueryItems", taskQueryItem);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeStarted notification, CancellationToken cancellationToken)
    {
        var taskQueryItem = await _db.GetAsync<TaskQueryItem>("TaskQueryItems", notification.TaskId);

        taskQueryItem.HasFocus = true;

        await _db.PutAsync("TaskQueryItems", taskQueryItem);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeInterrupted notification, CancellationToken cancellationToken)
    {
        var taskQueryItem = await _db.GetAsync<TaskQueryItem>("TaskQueryItems", notification.TaskId);

        if (taskQueryItem == null)
        {
            throw new EntityNotFoundException();
        }

        taskQueryItem.TotalHours += notification.Worktime.TotalHours;

        taskQueryItem.HasFocus = false;

        await _db.PutAsync("TaskQueryItems", taskQueryItem);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeStopped notification, CancellationToken cancellationToken)
    {
        var taskQueryItem = await _db.GetAsync<TaskQueryItem>("TaskQueryItems", notification.TaskId);

        if (taskQueryItem == null)
        {
            throw new EntityNotFoundException();
        }

        taskQueryItem.TotalHours += notification.Worktime.TotalHours;

        taskQueryItem.HasFocus = false;

        await _db.PutAsync("TaskQueryItems", taskQueryItem);
    }

    public async System.Threading.Tasks.Task Handle(TaskArchived notification, CancellationToken cancellationToken)
    {
        var taskQueryItem = await _db.GetAsync<TaskQueryItem>("TaskQueryItems", notification.Id);

        if (taskQueryItem == null)
        {
            throw new EntityNotFoundException();
        }

        await _db.RemoveAsync("TaskQueryItems", notification.Id);
    }
}
