namespace Pomodorium.Modules.Flows;

public class TaskQueryItem
{
    public Guid Id { get; set; }

    public DateTime? CreationDate { get; set; }

    public string? Description { get; set; }

    public double? TotalHours { get; set; }

    public long Version { get; set; }
}
