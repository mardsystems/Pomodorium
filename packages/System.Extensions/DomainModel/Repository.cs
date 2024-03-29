﻿using MediatR;
using System.Collections.ObjectModel;
using System.DomainModel.Storage;

namespace System.DomainModel;

public class Repository
{
    private readonly EventStore _eventStore;

    private readonly IMediator _mediator;

    public Repository(EventStore eventStore, IMediator mediator)
    {
        _eventStore = eventStore;
        _mediator = mediator;
    }

    public async Task<ObservableCollection<TAggregate>> GetAll<TAggregate>()
        where TAggregate : AggregateRoot, new()
    {
        var aggregates = new ObservableCollection<TAggregate>();

        var events = await _eventStore.LoadAllEvents();

        foreach (var _ in events)
        {
            try
            {
                //if (@event is PomodoroCreated)
                //{
                //    var pomodoro = new Pomodoro(new[] { @event });

                //    aggregates.Add(pomodoro);
                //}
            }
            catch (Exception)
            {

            }
        }

        return aggregates;
    }

    public async Task<TAggregate> GetAggregateById<TAggregate>(Guid id)
        where TAggregate : AggregateRoot, new()
    {
        var events = await _eventStore.GetEventsForAggregate(id, EventStore.IRRELEVANT_VERSION, long.MaxValue);

        var aggregate = new TAggregate();

        aggregate.LoadsFromHistory(events);

        return aggregate;
    }

    public async Task Save<TAggregate>(TAggregate aggregate, long originalVersion = EventStore.IRRELEVANT_VERSION)
        where TAggregate : AggregateRoot
    {
        try
        {
            await _eventStore.AppendToStream(aggregate.Id, originalVersion, aggregate.Changes.ToArray());

            aggregate.Version = originalVersion + aggregate.Changes.Count;
        }
        catch (EventStoreConcurrencyException ex)
        {
            foreach (var failedEvent in aggregate.Changes)
            {
                foreach (var succededEvent in ex.StoreEvents)
                {
                    if (ConflictsWith(failedEvent, succededEvent))
                    {
                        var message = $"Conflict between ${failedEvent} and {succededEvent}";

                        throw new RealConcurrencyException(ex);
                    }
                }
            }

            await _eventStore.AppendToStream(aggregate.Id, ex.StoreVersion, aggregate.Changes.ToArray());

            aggregate.Version = ex.StoreVersion + aggregate.Changes.Count;
        }

        foreach (var @event in aggregate.Changes)
        {
            await _mediator.Publish(@event);
        }
    }

    private static bool ConflictsWith(Event event1, Event event2)
    {
        return event1.GetType() == event2.GetType();
    }
}
