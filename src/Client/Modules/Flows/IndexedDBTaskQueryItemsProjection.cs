using MediatR;
using Pomodorium.Data;
using System.DomainModel;
using System.Threading.Tasks;

namespace Pomodorium.Modules.Flows;

public class IndexedDBTaskQueryItemsProjection :
    IRequestHandler<GetTasksRequest, GetTasksResponse>,
    INotificationHandler<TaskCreated>,
    INotificationHandler<TaskDescriptionChanged>,
    INotificationHandler<FlowtimeInterrupted>,
    INotificationHandler<FlowtimeStopped>,
    INotificationHandler<TaskArchived>
{
    private readonly IndexedDBAccessor _db;

    private readonly Repository _repository;

    public IndexedDBTaskQueryItemsProjection(IndexedDBAccessor db, Repository repository)
    {
        _db = db;

        _repository = repository;
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

    public async System.Threading.Tasks.Task Handle(TaskDescriptionChanged notification, CancellationToken cancellationToken)
    {
        var taskQueryItem = await _db.GetAsync<TaskQueryItem>("TaskQueryItems", notification.Id);

        taskQueryItem.Description = notification.Description;

        await _db.PutAsync("TaskQueryItems", taskQueryItem);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeInterrupted notification, CancellationToken cancellationToken)
    {
        var flowtime = await _repository.GetAggregateById<Flowtime>(notification.Id);

        if (flowtime == null)
        {
            throw new EntityNotFoundException();
        }

        var taskQueryItem = await _db.GetAsync<TaskQueryItem>("TaskQueryItems", flowtime.TaskId);

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

        await _db.PutAsync("TaskQueryItems", taskQueryItem);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeStopped notification, CancellationToken cancellationToken)
    {
        var flowtime = await _repository.GetAggregateById<Flowtime>(notification.Id);

        if (flowtime == null)
        {
            throw new EntityNotFoundException();
        }

        var taskQueryItem = await _db.GetAsync<TaskQueryItem>("TaskQueryItems", flowtime.TaskId);

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
