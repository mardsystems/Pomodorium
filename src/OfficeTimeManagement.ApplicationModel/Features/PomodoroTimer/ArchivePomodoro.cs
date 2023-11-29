namespace Pomodorium.Features.PomodoroTimer;

public class ArchivePomodoroRequest : Request<ArchivePomodoroResponse>
{
    public Guid Id { get; set; }

    public long Version { get; set; }
}

public class ArchivePomodoroResponse : Response
{
    public ArchivePomodoroResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public ArchivePomodoroResponse()
    {

    }
}
