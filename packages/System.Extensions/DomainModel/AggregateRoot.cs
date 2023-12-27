namespace System.DomainModel;

public abstract class AggregateRoot : Entity
{
    public Guid Id { get; protected set; }

    public string UserId { get; protected set; } = default!;

    public DateTime CreationDate { get; protected set; }

    public bool Archived { get; private set; }

    public long Version { get; internal set; }

    public ICollection<Event> Changes { get; private set; }

    protected AggregateRoot(Guid id, AuditInterface auditInterface)
    {
        Id = id;

        UserId = auditInterface.GetUserId();

        CreationDate = auditInterface.GetCreationDate();

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

        Version = e.Version;
    }

    public virtual void Archive()
    {
        Archived = true;
    }

    //public void OnSave()
    //{
    //    OriginalVersion = Version;
    //}
}
