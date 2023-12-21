namespace Pomodorium.Features.TaskManager;

public record TaskArchiveRequest : Request<TaskArchiveResponse>
{
    public Guid TaskId { get; init; }

    public long TaskVersion { get; init; }
}

public record TaskArchiveResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required long TaskVersion { get; init; }
}
