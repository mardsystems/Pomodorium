namespace Pomodorium.Features.FlowTimer;

public record InterruptFlowtimeRequest : Request<InterruptFlowtimeResponse>
{
    public Guid FlowtimeId { get; init; }

    public long FlowtimeVersion { get; init; }
}

public record InterruptFlowtimeResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required long FlowtimeVersion { get; init; }
}
