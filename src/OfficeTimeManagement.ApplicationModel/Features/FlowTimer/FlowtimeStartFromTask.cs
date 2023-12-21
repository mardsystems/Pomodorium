namespace Pomodorium.Features.FlowTimer;

public record FlowtimeStartFromTaskRequest : Request<FlowtimeStartFromTaskResponse>
{
    public Guid TaskId { get; init; }
}

public record FlowtimeStartFromTaskResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required long FlowtimeVersion { get; init; }
}
