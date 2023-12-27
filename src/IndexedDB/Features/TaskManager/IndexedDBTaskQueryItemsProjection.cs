using MediatR;
using Pomodorium.Data;
using Pomodorium.Models.FlowtimeTechnique;
using Pomodorium.Models.TaskManagement.Integrations;
using Pomodorium.Models.TaskManagement.Tasks;
using System.DomainModel;

namespace Pomodorium.Features.TaskManager;

public class IndexedDBTaskQueryItemsProjection :
    IRequestHandler<TaskQueryRequest, TaskQueryResponse>,
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

    public async Task<TaskQueryResponse> Handle(TaskQueryRequest request, CancellationToken cancellationToken)
    {
        var taskQueryItems = await _db.GetAllAsync<TaskQueryItem>("TaskQueryItems");

        var orderedTaskQueryItems = taskQueryItems.OrderByDescending(x => x.CreationDate);

        var response = new TaskQueryResponse(request.GetCorrelationId())
        {
            TaskQueryItems = orderedTaskQueryItems
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
        var taskQueryItem = await _db.GetAsync<TaskQueryItem>("TaskQueryItems", notification.TaskId);

        taskQueryItem.Description = notification.TaskDescription;

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
        var taskQueryItem = await _db.GetAsync<TaskQueryItem>("TaskQueryItems", notification.TaskId) ?? throw new EntityNotFoundException();

        taskQueryItem.TotalHours += notification.Worktime.TotalHours;

        taskQueryItem.HasFocus = false;

        await _db.PutAsync("TaskQueryItems", taskQueryItem);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeStopped notification, CancellationToken cancellationToken)
    {
        var taskQueryItem = await _db.GetAsync<TaskQueryItem>("TaskQueryItems", notification.TaskId) ?? throw new EntityNotFoundException();

        taskQueryItem.TotalHours += notification.Worktime.TotalHours;

        taskQueryItem.HasFocus = false;

        await _db.PutAsync("TaskQueryItems", taskQueryItem);
    }

    public async System.Threading.Tasks.Task Handle(TaskArchived notification, CancellationToken cancellationToken)
    {
        var _ = await _db.GetAsync<TaskQueryItem>("TaskQueryItems", notification.TaskId) ?? throw new EntityNotFoundException();

        await _db.RemoveAsync("TaskQueryItems", notification.TaskId);
    }
}
