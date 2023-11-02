using System.DomainModel;

namespace Pomodorium.Modules.Flows;

public class Task : AggregateRoot
{
    public string Description { get; private set; }

    public Task(string description)
    {
        if (description == null)
        {
            throw new ArgumentNullException(nameof(description));
        }

        Apply(new TaskCreated(Id, description));
    }

    public void When(TaskCreated e)
    {
        Id = e.Id;

        Description = e.Description;
    }

    public Task() { }
}
