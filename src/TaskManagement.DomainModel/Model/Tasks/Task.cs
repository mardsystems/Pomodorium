namespace Pomodorium.TaskManagement.Model.Tasks;

public class Task : AggregateRoot
{
    public string Description { get; private set; }

    public string ExternalSourceId { get; private set; }

    public Task(string description, string externalSourceId)
    {
        if (description == null)
        {
            throw new ArgumentNullException(nameof(description));
        }

        Apply(new TaskCreated(Id, DateTime.Now, description, externalSourceId));
    }

    public void When(TaskCreated e)
    {
        Id = e.Id;

        CreationDate = e.CreationDate;

        Description = e.Description;

        ExternalSourceId = e.ExternalSourceId;
    }

    public void ChangeDescription(string description)
    {
        Apply(new TaskDescriptionChanged(Id, description));
    }

    public void When(TaskDescriptionChanged e)
    {
        Description = e.Description;
    }

    public Task() { }
}
