﻿using Pomodorium.Features.FlowTimer;
using Pomodorium.Models.Flows;
using MediatR;
using MongoDB.Driver;
using Pomodorium.Models.Tasks;

namespace Pomodorium.Data;

public class MongoDBFlowtimeQueryItemsProjection :
    IRequestHandler<FlowtimeQueryRequest, FlowtimeQueryResponse>,
    INotificationHandler<FlowtimeCreated>,
    INotificationHandler<FlowtimeStarted>,
    INotificationHandler<FlowtimeInterrupted>,
    INotificationHandler<FlowtimeStopped>,
    INotificationHandler<TaskDescriptionChanged>,
    INotificationHandler<FlowtimeArchived>
{
    private readonly MongoClient _mongoClient;

    private readonly IMongoCollection<FlowtimeQueryItem> _mongoCollection;

    public MongoDBFlowtimeQueryItemsProjection(MongoClient mongoClient)
    {
        _mongoClient = mongoClient;

        _mongoCollection = _mongoClient.GetDatabase("Pomodorium").GetCollection<FlowtimeQueryItem>("FlowtimeQueryItems");
    }

    public async Task<FlowtimeQueryResponse> Handle(FlowtimeQueryRequest request, CancellationToken cancellationToken)
    {
        var filter = Builders<FlowtimeQueryItem>.Filter.Empty;

        var flowtimeQueryItems = await _mongoCollection.Find(filter).ToListAsync(cancellationToken);

        var response = new FlowtimeQueryResponse(request.GetCorrelationId())
        {
            FlowtimeQueryItems = flowtimeQueryItems
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

        await _mongoCollection.InsertOneAsync(flowtimeQueryItem, null, cancellationToken);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeStarted notification, CancellationToken cancellationToken)
    {
        var filter = Builders<FlowtimeQueryItem>.Filter.Eq(x => x.Id, notification.FlowtimeId);

        var update = Builders<FlowtimeQueryItem>.Update
            .Set(x => x.StartDateTime, notification.StartedAt)
            .Set(x => x.State, notification.FlowtimeState)
            .Set(x => x.Version, notification.Version);

        await _mongoCollection.UpdateOneAsync(filter, update, null, cancellationToken);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeInterrupted notification, CancellationToken cancellationToken)
    {
        var filter = Builders<FlowtimeQueryItem>.Filter.Eq(x => x.Id, notification.Id);

        var update = Builders<FlowtimeQueryItem>.Update
            .Set(x => x.StopDateTime, notification.StopDateTime)
            .Set(x => x.Interrupted, notification.Interrupted)
            .Set(x => x.Worktime, notification.Worktime)
            .Set(x => x.Breaktime, notification.Breaktime)
            .Set(x => x.State, notification.State)
            .Set(x => x.Version, notification.Version);

        await _mongoCollection.UpdateOneAsync(filter, update, null, cancellationToken);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeStopped notification, CancellationToken cancellationToken)
    {
        var filter = Builders<FlowtimeQueryItem>.Filter.Eq(x => x.Id, notification.Id);

        var update = Builders<FlowtimeQueryItem>.Update
            .Set(x => x.StopDateTime, notification.StopDateTime)
            .Set(x => x.Interrupted, notification.Interrupted)
            .Set(x => x.Worktime, notification.Worktime)
            .Set(x => x.Breaktime, notification.Breaktime)
            .Set(x => x.State, notification.State)
            .Set(x => x.Version, notification.Version);

        await _mongoCollection.UpdateOneAsync(filter, update, null, cancellationToken);
    }

    public async System.Threading.Tasks.Task Handle(TaskDescriptionChanged notification, CancellationToken cancellationToken)
    {
        var filter = Builders<FlowtimeQueryItem>.Filter.Eq(x => x.TaskId, notification.TaskId);

        var update = Builders<FlowtimeQueryItem>.Update
            .Set(x => x.TaskDescription, notification.TaskDescription);

        await _mongoCollection.UpdateManyAsync(filter, update, null, cancellationToken);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeArchived notification, CancellationToken cancellationToken)
    {
        var filter = Builders<FlowtimeQueryItem>.Filter.Eq(x => x.Id, notification.Id);

        await _mongoCollection.DeleteOneAsync(filter, cancellationToken);
    }
}
