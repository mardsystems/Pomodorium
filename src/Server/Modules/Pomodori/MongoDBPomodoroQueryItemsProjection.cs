﻿using MediatR;
using MongoDB.Driver;
using System.DomainModel;

namespace Pomodorium.Modules.Pomodori;

public class MongoDBPomodoroQueryItemsProjection :
    IRequestHandler<GetPomodoriRequest, GetPomodoriResponse>,
    INotificationHandler<PomodoroCreated>,
    INotificationHandler<PomodoroDescriptionChanged>,
    INotificationHandler<PomodoroArchived>
{
    private readonly MongoClient _mongoClient;

    private readonly IMongoCollection<PomodoroQueryItem> _mongoCollection;

    public MongoDBPomodoroQueryItemsProjection(MongoClient mongoClient)
    {
        _mongoClient = mongoClient;

        _mongoCollection = _mongoClient.GetDatabase("Pomodorium").GetCollection<PomodoroQueryItem>("PomodoroQueryItems");
    }

    public async Task<GetPomodoriResponse> Handle(GetPomodoriRequest request, CancellationToken cancellationToken)
    {
        var filter = Builders<PomodoroQueryItem>.Filter.Empty;

        var pomodoroQueryItems = await _mongoCollection.Find(filter).ToListAsync(cancellationToken);

        var response = new GetPomodoriResponse(request.GetCorrelationId()) { PomodoroQueryItems = pomodoroQueryItems };

        return await Task.FromResult(response);
    }

    public async Task Handle(PomodoroCreated request, CancellationToken cancellationToken)
    {
        var pomodoroQueryItem = new PomodoroQueryItem
        {
            Id = request.Id.Value,
            StartDateTime = request.StartDateTime,
            Description = request.Description
        };

        await _mongoCollection.InsertOneAsync(pomodoroQueryItem, null, cancellationToken);
    }

    public async Task Handle(PomodoroDescriptionChanged notification, CancellationToken cancellationToken)
    {
        var filter = Builders<PomodoroQueryItem>.Filter.Eq(x => x.Id, notification.Id.Value);

        var pomodoroQueryItem = await _mongoCollection.Find(filter).FirstAsync(cancellationToken);

        if (pomodoroQueryItem == null)
        {
            throw new EntityNotFoundException();
        }

        pomodoroQueryItem.Description = notification.Description;

        var update = Builders<PomodoroQueryItem>.Update
            .Set(x => x.Description, notification.Description);

        await _mongoCollection.UpdateOneAsync(filter, update, null, cancellationToken);
    }

    public async Task Handle(PomodoroArchived notification, CancellationToken cancellationToken)
    {
        var filter = Builders<PomodoroQueryItem>.Filter.Eq(x => x.Id, notification.Id.Value);

        var pomodoroQueryItem = await _mongoCollection.Find(filter).FirstAsync(cancellationToken);

        if (pomodoroQueryItem == null)
        {
            throw new EntityNotFoundException();
        }

        await _mongoCollection.DeleteOneAsync(filter, cancellationToken);
    }
}