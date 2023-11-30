namespace Pomodorium.Features.TaskManager;

public class ArchiveTaskRequest : Request<ArchiveTaskResponse>
{
    public Guid TaskId { get; set; }

    public long TaskVersion { get; set; }
}

public class ArchiveTaskResponse : Response
{
    public ArchiveTaskResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public ArchiveTaskResponse() { }
}
