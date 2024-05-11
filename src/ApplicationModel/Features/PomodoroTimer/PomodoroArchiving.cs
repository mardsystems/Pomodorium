namespace Pomodorium.Features.PomodoroTimer;

public record PomodoroArchivingRequest : Request<PomodoroArchivingResponse>
{
    public Guid Id { get; init; }

    public long Version { get; init; }
}

public record PomodoroArchivingResponse(Guid CorrelationId) : Response(CorrelationId)
{

}
