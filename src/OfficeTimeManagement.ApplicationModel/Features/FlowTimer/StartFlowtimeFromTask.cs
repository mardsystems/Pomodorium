namespace Pomodorium.Features.FlowTimer;

public record StartFlowtimeFromTaskRequest : Request<StartFlowtimeFromTaskResponse>
{
    public Guid TaskId { get; init; }
}

public record StartFlowtimeFromTaskResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required long FlowtimeVersion { get; init; }
}
