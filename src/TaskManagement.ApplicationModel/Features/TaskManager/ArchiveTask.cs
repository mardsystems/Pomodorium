namespace Pomodorium.Features.TaskManager;

public class ArchiveTaskRequest : Request<ArchiveTaskResponse>
{
    public Guid Id { get; set; }

    public long Version { get; set; }
}

public class ArchiveTaskResponse : Response
{
    public ArchiveTaskResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public ArchiveTaskResponse() { }
}
