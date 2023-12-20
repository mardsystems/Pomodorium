namespace Pomodorium.Features.FlowTimer;

public record StartFlowtimeRequest : Request<StartFlowtimeResponse>
{
    public Guid FlowtimeId { get; init; }

    public long FlowtimeVersion { get; init; }
}

public record StartFlowtimeResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required long FlowtimeVersion { get; init; }
}
