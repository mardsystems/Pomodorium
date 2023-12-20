namespace Pomodorium.Features.PomodoroTimer;

public record ArchivePomodoroRequest : Request<ArchivePomodoroResponse>
{
    public Guid Id { get; init; }

    public long Version { get; init; }
}

public record ArchivePomodoroResponse(Guid CorrelationId) : Response(CorrelationId)
{

}
