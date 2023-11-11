namespace Pomodorium.Modules.Flows;

public class ArchiveTaskRequest : Request<ArchiveTaskResponse>
{
    public Guid Id { get; set; }

    public long Version { get; set; }
}
