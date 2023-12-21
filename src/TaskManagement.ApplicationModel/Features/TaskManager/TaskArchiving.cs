namespace Pomodorium.Features.TaskManager;

public record TaskArchivingRequest : Request<TaskArchivingResponse>
{
    public required Guid TaskId { get; init; }

    public long TaskVersion { get; init; }
}

public record TaskArchivingResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required long TaskVersion { get; init; }
}
