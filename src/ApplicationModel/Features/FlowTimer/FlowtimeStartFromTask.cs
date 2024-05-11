namespace Pomodorium.Features.FlowTimer;

public record FlowtimeStartFromTaskRequest : Request<FlowtimeStartFromTaskResponse>
{
    public required Guid TaskId { get; init; }
}

public record FlowtimeStartFromTaskResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required long FlowtimeVersion { get; init; }
}
