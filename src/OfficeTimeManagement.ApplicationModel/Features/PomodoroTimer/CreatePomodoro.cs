namespace Pomodorium.Features.PomodoroTimer;

public record CreatePomodoroRequest : Request<CreatePomodoroResponse>
{
    public required string Task { get; init; }

    public TimeSpan Timer { get; init; }
}

public record CreatePomodoroResponse(Guid CorrelationId) : Response(CorrelationId)
{

}
