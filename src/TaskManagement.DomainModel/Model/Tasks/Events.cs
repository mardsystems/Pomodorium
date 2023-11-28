using System.Runtime.Serialization;

namespace Pomodorium.TaskManagement.Model.Tasks;

[DataContract]
public class TaskCreated : Event
{
    [DataMember(Order = 1)]
    public Guid Id { get; private set; }

    [DataMember(Order = 2)]
    public DateTime CreationDate { get; private set; }

    [DataMember(Order = 3)]
    public string Description { get; private set; }

    [DataMember(Order = 4)]
    public string ExternalSourceId { get; private set; }

    public TaskCreated(Guid id, DateTime creationDate, string description, string externalSourceId)
    {
        Id = id;

        CreationDate = creationDate;

        Description = description;

        ExternalSourceId = externalSourceId;
    }

    private TaskCreated() { }
}

[DataContract]
public class TaskDescriptionChanged : Event
{
    [DataMember(Order = 1)]
    public Guid Id { get; private set; }

    [DataMember(Order = 2)]
    public string? Description { get; private set; }

    public TaskDescriptionChanged(Guid id, string description)
    {
        Id = id;

        Description = description;
    }

    private TaskDescriptionChanged() { }
}

[DataContract]
public class TaskArchived : Event
{
    [DataMember(Order = 1)]
    public Guid Id { get; private set; }

    public TaskArchived(Guid id)
    {
        Id = id;
    }

    private TaskArchived() { }
}
