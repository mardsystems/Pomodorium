using System.DomainModel;

namespace Pomodorium.Modules.Flows;

public class Task : AggregateRoot
{
    public string Description { get; private set; }

    public Task(string description)
    {
        Description = description;
    }
}
