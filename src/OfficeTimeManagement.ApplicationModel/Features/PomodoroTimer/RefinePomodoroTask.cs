namespace Pomodorium.Features.PomodoroTimer;

public record RefinePomodoroTaskRequest : Request<RefinePomodoroTaskResponse>
{
    public Guid Id { get; init; }

    public required string Task { get; init; }

    public long Version { get; init; }
}

public record RefinePomodoroTaskResponse(Guid CorrelationId) : Response(CorrelationId)
{

}
