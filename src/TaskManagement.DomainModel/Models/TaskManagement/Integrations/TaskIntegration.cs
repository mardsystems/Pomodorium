namespace Pomodorium.Models.TaskManagement.Integrations;

public class TaskIntegration : AggregateRoot
{
    public Guid TaskId { get; private set; }

    public string ExternalReference { get; private set; } = default!;

    public TaskIntegration(Tasks.Task task, TaskInfo taskInfo)
    {
        if (task == null)
        {
            throw new ArgumentNullException(nameof(task));
        }

        if (taskInfo == null)
        {
            throw new ArgumentNullException(nameof(taskInfo));
        }

        Apply(new TaskIntegrated(Id, task.Id, taskInfo.IntegrationType, taskInfo.IntegrationId, taskInfo.IntegrationName, taskInfo.Reference));
    }

    public void When(TaskIntegrated e)
    {
        Id = e.TaskIntegrationId;

        TaskId = e.TaskId;

        ExternalReference = e.ExternalReference;
    }

    private TaskIntegration() { }
}
