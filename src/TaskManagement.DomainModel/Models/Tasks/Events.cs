using System.DomainModel;
using System.Runtime.Serialization;

namespace TaskManagement.Models.Tasks;

[DataContract]
public class TaskCreated : Event
{
    [DataMember(Order = 1)]
    public Guid TaskId { get; private set; }

    [DataMember(Order = 4)]
    public string UserId { get; private set; } = default!;

    [DataMember(Order = 2)]
    public DateTime TaskCreatedAt { get; private set; }

    [DataMember(Order = 3)]
    public string TaskDescription { get; private set; } = default!;

    public TaskCreated(Guid taskId, AuditInterface auditInterface, string taskDescription)
    {
        TaskId = taskId;

        UserId = auditInterface.GetUserId();

        TaskCreatedAt = auditInterface.GetCreationDate();

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
[Obsolete("Use TaskArchived", true)]
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

[DataContract(Name = "TaskArchivingd")]
public class TaskArchived : Event
{
    [DataMember(Order = 1)]
    public Guid TaskId { get; private set; }

    public TaskArchived(Guid taskId)
    {
        TaskId = taskId;
    }

    private TaskArchived() { }
}
