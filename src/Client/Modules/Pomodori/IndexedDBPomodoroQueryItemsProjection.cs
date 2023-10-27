﻿using MediatR;
using Pomodorium.Data;
using System.DomainModel;

namespace Pomodorium.Modules.Pomodori;

public class IndexedDBPomodoroQueryItemsProjection :
    IRequestHandler<GetPomodoriRequest, GetPomodoriResponse>,
    INotificationHandler<PomodoroCreated>,
    INotificationHandler<PomodoroDescriptionChanged>,
    INotificationHandler<PomodoroArchived>
{
    private readonly IndexedDBAccessor _db;

    public IndexedDBPomodoroQueryItemsProjection(IndexedDBAccessor db)
    {
        _db = db;
    }

    public async Task<GetPomodoriResponse> Handle(GetPomodoriRequest request, CancellationToken cancellationToken)
    {
        var pomodoroQueryItems = await _db.GetAllAsync<PomodoroQueryItem>("PomodoroQueryItems");

        var response = new GetPomodoriResponse(request.GetCorrelationId()) { PomodoroQueryItems = pomodoroQueryItems };

        return response;
    }

    public async Task Handle(PomodoroCreated request, CancellationToken cancellationToken)
    {
        var pomodoroQueryItem = new PomodoroQueryItem
        {
            Id = request.Id.Value,
            StartDateTime = request.StartDateTime,
            Description = request.Description
        };

        await _db.PutAsync("PomodoroQueryItems", pomodoroQueryItem);
    }

    public async Task Handle(PomodoroDescriptionChanged notification, CancellationToken cancellationToken)
    {
        var pomodoroQueryItem = await _db.GetAsync<PomodoroQueryItem>("PomodoroQueryItems", notification.Id.Value);

        if (pomodoroQueryItem == null)
        {
            throw new EntityNotFoundException();
        }

        pomodoroQueryItem.Description = notification.Description;

        await _db.PutAsync("PomodoroQueryItems", pomodoroQueryItem);
    }

    public async Task Handle(PomodoroArchived notification, CancellationToken cancellationToken)
    {
        var pomodoroQueryItem = await _db.GetAsync<PomodoroDetails>("PomodoroQueryItems", notification.Id.Value);

        if (pomodoroQueryItem == null)
        {
            throw new EntityNotFoundException();
        }

        await _db.RemoveAsync("PomodoroQueryItems", notification.Id.Value);
    }
}
