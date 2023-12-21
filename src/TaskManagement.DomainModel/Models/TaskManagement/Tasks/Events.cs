using System.Runtime.Serialization;

namespace Pomodorium.Models.TaskManagement.Tasks;

[DataContract]
public class TaskCreated : Event
{
    [DataMember(Order = 1)]
    public Guid TaskId { get; private set; }

    [DataMember(Order = 2)]
    public DateTime TaskCreatedAt { get; private set; }

    [DataMember(Order = 3)]
    public string TaskDescription { get; private set; } = default!;

    public TaskCreated(Guid taskId, DateTime taskCreatedAt, string taskDescription)
    {
        TaskId = taskId;

        TaskCreatedAt = taskCreatedAt;

        TaskDescription = taskDescription;
    }

    private TaskCreated() { }
}

[DataContract]
public class TaskDescriptionChanged : Event
{
    [DataMember(Order = 1)]
    public Guid TaskId { get; private set; }

    [DataMember(Order = 2)]
    public string TaskDescription { get; private set; } = default!;

    public TaskDescriptionChanged(Guid taskId, string taskDescription)
    {
        TaskId = taskId;

        TaskDescription = taskDescription;
    }

    private TaskDescriptionChanged() { }
}

[DataContract]
public class TaskArchivingd : Event
{
    [DataMember(Order = 1)]
    public Guid TaskId { get; private set; }

    public TaskArchivingd(Guid taskId)
    {
        TaskId = taskId;
    }

    private TaskArchivingd() { }
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
