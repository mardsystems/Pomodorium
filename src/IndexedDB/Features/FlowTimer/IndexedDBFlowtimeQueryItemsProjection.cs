﻿using MediatR;
using Pomodorium.Data;
using Pomodorium.Models.FlowtimeTechnique;
using Pomodorium.Models.TaskManagement.Tasks;
using System.DomainModel;

namespace Pomodorium.Features.FlowTimer;

public class IndexedDBFlowtimeQueryItemsProjection :
    IRequestHandler<GetFlowsRequest, GetFlowsResponse>,
    INotificationHandler<FlowtimeCreated>,
    INotificationHandler<FlowtimeStarted>,
    INotificationHandler<FlowtimeInterrupted>,
    INotificationHandler<FlowtimeStopped>,
    INotificationHandler<TaskDescriptionChanged>,
    INotificationHandler<FlowtimeArchived>
{
    private readonly IndexedDBAccessor _db;

    public IndexedDBFlowtimeQueryItemsProjection(IndexedDBAccessor db)
    {
        _db = db;
    }

    public async Task<GetFlowsResponse> Handle(GetFlowsRequest request, CancellationToken cancellationToken)
    {
        var flowtimeQueryItems = await _db.GetAllAsync<FlowtimeQueryItem>("FlowtimeQueryItems");

        var orderedFlowtimeQueryItems = flowtimeQueryItems.OrderByDescending(x => x.CreationDate);

        var response = new GetFlowsResponse(request.GetCorrelationId())
        {
            FlowtimeQueryItems = orderedFlowtimeQueryItems
        };

        return response;
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeCreated notification, CancellationToken cancellationToken)
    {
        var flowtimeQueryItem = new FlowtimeQueryItem
        {
            Id = notification.Id,
            CreationDate = notification.CreationDate,
            State = notification.State,
            TaskId = notification.TaskId,
            TaskDescription = notification.TaskDescription,
            Version = notification.Version
        };

        await _db.PutAsync("FlowtimeQueryItems", flowtimeQueryItem);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeStarted notification, CancellationToken cancellationToken)
    {
        var flowtimeQueryItem = await _db.GetAsync<FlowtimeQueryItem>("FlowtimeQueryItems", notification.Id) ?? throw new EntityNotFoundException();

        flowtimeQueryItem.StartDateTime = notification.StartDateTime;
        flowtimeQueryItem.State = notification.State;
        flowtimeQueryItem.Version = notification.Version;

        await _db.PutAsync("FlowtimeQueryItems", flowtimeQueryItem);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeInterrupted notification, CancellationToken cancellationToken)
    {
        var flowtimeQueryItem = await _db.GetAsync<FlowtimeQueryItem>("FlowtimeQueryItems", notification.Id) ?? throw new EntityNotFoundException();

        flowtimeQueryItem.StopDateTime = notification.StopDateTime;
        flowtimeQueryItem.Interrupted = notification.Interrupted;
        flowtimeQueryItem.Worktime = notification.Worktime;
        flowtimeQueryItem.Breaktime = notification.Breaktime;
        flowtimeQueryItem.State = notification.State;
        flowtimeQueryItem.Version = notification.Version;

        await _db.PutAsync("FlowtimeQueryItems", flowtimeQueryItem);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeStopped notification, CancellationToken cancellationToken)
    {
        var flowtimeQueryItem = await _db.GetAsync<FlowtimeQueryItem>("FlowtimeQueryItems", notification.Id) ?? throw new EntityNotFoundException();

        flowtimeQueryItem.StopDateTime = notification.StopDateTime;
        flowtimeQueryItem.Interrupted = notification.Interrupted;
        flowtimeQueryItem.Worktime = notification.Worktime;
        flowtimeQueryItem.Breaktime = notification.Breaktime;
        flowtimeQueryItem.State = notification.State;
        flowtimeQueryItem.Version = notification.Version;

        await _db.PutAsync("FlowtimeQueryItems", flowtimeQueryItem);
    }

    public async System.Threading.Tasks.Task Handle(TaskDescriptionChanged notification, CancellationToken cancellationToken)
    {
        var flowtimeQueryItems = await _db.GetAllAsync<FlowtimeQueryItem>("FlowtimeQueryItems");

        var flowtimeQueryItemByTaskId = flowtimeQueryItems.Where(x => x.TaskId == notification.TaskId);

        foreach (var flowtimeQueryItem in flowtimeQueryItemByTaskId)
        {
            flowtimeQueryItem.TaskDescription = notification.TaskDescription;

            await _db.PutAsync("FlowtimeQueryItems", flowtimeQueryItem);
        }
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeArchived notification, CancellationToken cancellationToken)
    {
        var _ = await _db.GetAsync<FlowtimeQueryItem>("FlowtimeQueryItems", notification.Id) ?? throw new EntityNotFoundException();

        await _db.RemoveAsync("FlowtimeQueryItems", notification.Id);
    }
}
