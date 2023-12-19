namespace Pomodorium.Features.PomodoroTimer;

public record CheckPomodoroRequest : Request<CheckPomodoroResponse>
{
    public Guid Id { get; init; }

    public long Version { get; init; }
}

public record CheckPomodoroResponse(Guid CorrelationId) : Response(CorrelationId)
{

}
