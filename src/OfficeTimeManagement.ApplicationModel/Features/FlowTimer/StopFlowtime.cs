namespace Pomodorium.Features.FlowTimer;

public record StopFlowtimeRequest : Request<StopFlowtimeResponse>
{
    public Guid FlowtimeId { get; init; }

    public long FlowtimeVersion { get; init; }
}

public record StopFlowtimeResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required long FlowtimeVersion { get; init; }
}
