using System.DomainModel;
using System.Runtime.Serialization;

namespace Pomodorium.Modules.Timers;

[DataContract]
public class PomodoroCreated : Event
{
    [DataMember(Order = 1)]
    public Guid Id { get; private set; }

    [DataMember(Order = 2)]
    public DateTime StartDateTime { get; private set; }

    [DataMember(Order = 3)]
    public string? Description { get; private set; }

    public PomodoroCreated(Guid id, DateTime startDateTime, string description)
    {
        Id = id;

        StartDateTime = startDateTime;

        Description = description;
    }

    private PomodoroCreated()
    {

    }
}

[DataContract]
public class PomodoroDescriptionChanged : Event
{
    [DataMember(Order = 1)]
    public Guid Id { get; private set; }

    [DataMember(Order = 2)]
    public string? Description { get; private set; }

    public PomodoroDescriptionChanged(Guid id, string description)
    {
        Id = id;

        Description = description;
    }

    private PomodoroDescriptionChanged()
    {

    }
}

[DataContract]
public class PomodoroArchived : Event
{
    [DataMember(Order = 1)]
    public Guid Id { get; private set; }

    public PomodoroArchived(Guid id)
    {
        Id = id;
    }

    private PomodoroArchived()
    {

    }
}
