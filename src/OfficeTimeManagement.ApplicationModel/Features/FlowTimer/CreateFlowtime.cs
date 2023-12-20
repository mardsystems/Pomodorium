namespace Pomodorium.Features.FlowTimer;

public record CreateFlowtimeRequest : Request<CreateFlowtimeResponse>
{
    public required string TaskDescription { get; init; }
}

public record CreateFlowtimeResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required long FlowtimeVersion { get; init; }
}
