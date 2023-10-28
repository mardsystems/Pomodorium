using MediatR;
using System.Collections.ObjectModel;
using System.DomainModel;
using System.DomainModel.Storage;

namespace Pomodorium.Modules.Timers;

public class TimersRepository
{
    private readonly EventStore _eventStore;

    private readonly IMediator _mediator;

    public TimersRepository(EventStore eventStore, IMediator mediator)
    {
        _eventStore = eventStore;
        _mediator = mediator;
    }

    public async Task<ObservableCollection<Pomodoro>> GetPomodori()
    {
        var pomodori = new ObservableCollection<Pomodoro>();

        var events = await _eventStore.LoadAllEvents();

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
        var events = await _eventStore.GetEventsForAggregate(id.Value, 0, long.MaxValue);

        var pomodoro = new Pomodoro(events);

        return await Task.FromResult(pomodoro);
    }

    public async Task Save(Pomodoro pomodoro, long originalVersion)
    {
        try
        {
            await _eventStore.AppendToStream(pomodoro.Id.Value, originalVersion, pomodoro.Changes.ToArray());
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

            await _eventStore.AppendToStream(pomodoro.Id.Value, ex.StoreVersion, pomodoro.Changes.ToArray());
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
