namespace Pomodorium.Features.PomodoroTimer;

public record PomodoroTaskRefinementRequest : Request<PomodoroTaskRefinementResponse>
{
    public Guid Id { get; init; }

    public required string Task { get; init; }

    public long Version { get; init; }
}

public record PomodoroTaskRefinementResponse(Guid CorrelationId) : Response(CorrelationId)
{

}
