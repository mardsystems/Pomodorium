namespace Pomodorium.TaskManagement.TaskManager;

public class TaskDetails
{
    public Guid Id { get; set; }

    public DateTime? CreationDate { get; set; }

    public string? Description { get; set; }

    public long Version { get; set; }
}
