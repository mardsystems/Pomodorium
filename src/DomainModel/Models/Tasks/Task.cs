namespace Pomodorium.Models.Tasks;

public class Task : AggregateRoot
{
    public string Description { get; private set; } = default!;

    public Task(Guid id, string description, AuditInterface auditInterface)
        : base(id, auditInterface)
    {
        if (description == null)
        {
            throw new ArgumentNullException(nameof(description));
        }

        Apply(new TaskCreated(Id, auditInterface, description));
    }

    public void When(TaskCreated e)
    {
        Id = e.TaskId;

        UserId = e.UserId;

        CreationDate = e.TaskCreatedAt;

        Description = e.TaskDescription;
    }

    public void ChangeDescription(string description)
    {
        Apply(new TaskDescriptionChanged(Id, description));
    }

    public void When(TaskDescriptionChanged e)
    {
        Description = e.TaskDescription;
    }

    public override void Archive()
    {
        Apply(new TaskArchived(Id));
    }

    public void When(TaskArchived _)
    {
        base.Archive();
    }

    public Task() { }
}
