namespace Pomodorium.Features.FlowTimer;

public record FlowtimeStartRequest : Request<FlowtimeStartResponse>
{
    public required Guid FlowtimeId { get; init; }

    public long FlowtimeVersion { get; init; }
}

public record FlowtimeStartResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required long FlowtimeVersion { get; init; }
}
