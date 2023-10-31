namespace System.DomainModel;

public abstract class AggregateRoot : Entity
{
    public Guid Id { get; internal protected set; }

    public string UserId { get; private set; }

    public long Version { get; internal set; }

    public ICollection<Event> Changes { get; private set; }

    public AggregateRoot(Guid id, string userId)
    {
        Id = id;

        UserId = userId;

        Changes = new HashSet<Event>();
    }

    protected AggregateRoot()
    {
        Changes = new HashSet<Event>();
    }

    public void LoadsFromHistory(IEnumerable<Event> history)
    {
        foreach (var e in history)
        {
            Mutate(e);
        }
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

        //Version = e.Version;
    }

    protected void Apply(Event e)
    {
        Changes.Add(e);

        Mutate(e);

        //Version++;

        //e.Version = Version;

        //e.Date = DateTime.Now;
    }

    protected void Mutate(Event e)
    {
        ((dynamic)this).When((dynamic)e);
    }

    //public void OnSave()
    //{
    //    OriginalVersion = Version;
    //}
}
