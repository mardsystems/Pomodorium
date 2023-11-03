﻿using MediatR;
using Pomodorium.Data;
using System.DomainModel;

namespace Pomodorium.Modules.Flows;

public class IndexedDBFlowtimeDetailsProjection :
    IRequestHandler<GetFlowtimeRequest, GetFlowtimeResponse>,
    INotificationHandler<FlowtimeCreated>,
    INotificationHandler<FlowtimeStarted>,
    INotificationHandler<FlowtimeStopped>,
    INotificationHandler<TaskDescriptionChanged>,
    INotificationHandler<FlowtimeArchived>
{
    private readonly IndexedDBAccessor _db;

    public IndexedDBFlowtimeDetailsProjection(IndexedDBAccessor db)
    {
        _db = db;
    }

    public async Task<GetFlowtimeResponse> Handle(GetFlowtimeRequest request, CancellationToken cancellationToken)
    {
        var pomodoroDetails = await _db.GetAsync<FlowtimeDetails>("FlowtimeDetails", request.Id);

        if (pomodoroDetails == null)
        {
            throw new EntityNotFoundException();
        }

        var response = new GetFlowtimeResponse(request.GetCorrelationId()) { FlowtimeDetails = pomodoroDetails };

        return response;
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeCreated notification, CancellationToken cancellationToken)
    {
        var flowtimeDetails = new FlowtimeDetails
        {
            Id = notification.Id,
            State = notification.State,
            TaskId = notification.TaskId,
            TaskDescription = notification.TaskDescription,
            TaskVersion = notification.TaskVersion,
            Version = notification.Version
        };

        await _db.PutAsync("FlowtimeDetails", flowtimeDetails);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeStarted notification, CancellationToken cancellationToken)
    {
        var flowtimeDetails = await _db.GetAsync<FlowtimeDetails>("FlowtimeDetails", notification.Id);

        if (flowtimeDetails == null)
        {
            throw new EntityNotFoundException();
        }

        flowtimeDetails.StartDateTime = notification.StartDateTime;
        flowtimeDetails.State = notification.State;
        flowtimeDetails.Version = notification.Version;

        await _db.PutAsync("FlowtimeDetails", flowtimeDetails);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeStopped notification, CancellationToken cancellationToken)
    {
        var flowtimeDetails = await _db.GetAsync<FlowtimeDetails>("FlowtimeDetails", notification.Id);

        if (flowtimeDetails == null)
        {
            throw new EntityNotFoundException();
        }

        flowtimeDetails.StopDateTime = notification.StopDateTime;
        flowtimeDetails.Interrupted = notification.Interrupted;
        flowtimeDetails.Worktime = notification.Worktime;
        flowtimeDetails.Breaktime = notification.Breaktime;
        flowtimeDetails.State = notification.State;
        flowtimeDetails.Version = notification.Version;

        await _db.PutAsync("FlowtimeDetails", flowtimeDetails);
    }

    public async System.Threading.Tasks.Task Handle(TaskDescriptionChanged notification, CancellationToken cancellationToken)
    {
        var flowtimeDetails = await _db.GetAsync<FlowtimeDetails>("FlowtimeDetails", notification.Id);

        if (flowtimeDetails == null)
        {
            throw new EntityNotFoundException();
        }

        flowtimeDetails.TaskDescription = notification.Description;
        flowtimeDetails.Version = notification.Version;

        await _db.PutAsync("FlowtimeDetails", flowtimeDetails);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeArchived notification, CancellationToken cancellationToken)
    {
        var flowtimeDetails = await _db.GetAsync<FlowtimeDetails>("FlowtimeDetails", notification.Id);

        if (flowtimeDetails == null)
        {
            throw new EntityNotFoundException();
        }

        await _db.RemoveAsync("FlowtimeDetails", notification.Id);
    }
}
