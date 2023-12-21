namespace Pomodorium.Features.FlowTimer;

public record FlowtimeInterruptionRequest : Request<FlowtimeInterruptionResponse>
{
    public Guid FlowtimeId { get; init; }

    public long FlowtimeVersion { get; init; }
}

public record FlowtimeInterruptionResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required long FlowtimeVersion { get; init; }
}
