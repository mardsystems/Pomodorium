namespace Pomodorium.Features.TaskManager;

public record ArchiveTaskRequest : Request<ArchiveTaskResponse>
{
    public Guid TaskId { get; init; }

    public long TaskVersion { get; init; }
}

public record ArchiveTaskResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required long TaskVersion { get; init; }
}
