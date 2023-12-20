namespace Pomodorium.Features.FlowTimer;

public record CreateFlowtimeFromTaskRequest : Request<CreateFlowtimeFromTaskResponse>
{
    public Guid TaskId { get; init; }

    public required string TaskDescription { get; init; }

    public long TaskVersion { get; init; }
}

public record CreateFlowtimeFromTaskResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required long FlowtimeVersion { get; init; }
}
