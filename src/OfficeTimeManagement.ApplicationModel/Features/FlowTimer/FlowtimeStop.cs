namespace Pomodorium.Features.FlowTimer;

public record FlowtimeStopRequest : Request<FlowtimeStopResponse>
{
    public required Guid FlowtimeId { get; init; }

    public long FlowtimeVersion { get; init; }
}

public record FlowtimeStopResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required long FlowtimeVersion { get; init; }
}
