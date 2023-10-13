namespace Pomodorium;

public abstract class AggregateRoot : Entity
{

    public string Id { get; internal protected set; }

    public string UserId { get; private set; }

    public long OriginalVersion { get; set; }

    public long CurrentVersion { get; set; }

    public ICollection<Event> Changes { get; private set; }

    public AggregateRoot(string id, string userId)
    {
        Id = id;

        UserId = userId;

        Changes = new HashSet<Event>();
    }

    protected AggregateRoot()
    {
        Changes = new HashSet<Event>();
    }

    public void Replay(IEnumerable<Event> events)
    {
        foreach (var @event in events)
        {
            Replay(@event);
        }
    }

    public void Replay(Event e)
    {
        Mutate(e);

        OriginalVersion = e.Version;

        CurrentVersion = OriginalVersion;
    }

    protected void Apply(Event e)
    {
        Changes.Add(e);

        Mutate(e);

        CurrentVersion++;

        e.Version = CurrentVersion;

        e.Date = DateTime.Now;
    }

    protected void Mutate(Event e)
    {
        ((dynamic)this).When((dynamic)e);
    }

    public void OnSave()
    {
        OriginalVersion = CurrentVersion;
    }
}
