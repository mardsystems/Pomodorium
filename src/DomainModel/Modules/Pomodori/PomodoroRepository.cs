using MediatR;
using System.Collections.ObjectModel;
using System.DomainModel;
using System.DomainModel.EventStore;
using System.Xml.Linq;

namespace Pomodorium.Modules.Pomodori;

public class PomodoroRepository
{
    private readonly EventStoreRepository _eventStore;

    private readonly IMediator _mediator;

    public PomodoroRepository(EventStoreRepository eventStore, IMediator mediator)
    {
        _eventStore = eventStore;
        _mediator = mediator;
    }

    public async Task<ObservableCollection<Pomodoro>> GetPomodori()
    {
        var pomodori = new ObservableCollection<Pomodoro>();

        var events = _eventStore.LoadAllEvents();

        foreach (var @event in events)
        {
            try
            {
                if (@event is PomodoroCreated)
                {
                    var pomodoro = new Pomodoro(new[] { @event });

                    pomodori.Add(pomodoro);
                }
            }
            catch (Exception)
            {

            }
        }

        return await Task.FromResult(pomodori);
    }

    public async Task<Pomodoro> GetPomodoroById(PomodoroId id)
    {
        var events = _eventStore.LoadEvents(id, 0, long.MaxValue);

        var pomodoro = new Pomodoro(events);

        return await Task.FromResult(pomodoro);
    }

    public async Task Add(Pomodoro pomodoro)
    {
        try
        {
            await _eventStore.AppendToStream(pomodoro.Id, pomodoro.OriginalVersion, pomodoro.Changes.ToArray());
        }
        catch (EventStoreConcurrencyException ex)
        {
            foreach (var failedEvent in pomodoro.Changes)
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

            await _eventStore.AppendToStream(pomodoro.Id, ex.StoreVersion, pomodoro.Changes.ToArray());
        }

        foreach (var @event in pomodoro.Changes)
        {
            await _mediator.Publish(@event);
        }
    }

    public async Task Update(Pomodoro pomodoro)
    {
        try
        {
            await _eventStore.AppendToStream(pomodoro.Id, pomodoro.OriginalVersion, pomodoro.Changes.ToArray());
        }
        catch (EventStoreConcurrencyException ex)
        {
            foreach (var failedEvent in pomodoro.Changes)
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

            await _eventStore.AppendToStream(pomodoro.Id, ex.StoreVersion, pomodoro.Changes.ToArray());
        }

        foreach (var @event in pomodoro.Changes)
        {
            await _mediator.Publish(@event);
        }
    }

    private bool ConflictsWith(Event event1, Event event2)
    {
        return event1.GetType() == event2.GetType();
    }
}
