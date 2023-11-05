namespace Pomodorium.Modules.Pomos;

public class ArchivePomodoroRequest : Request<ArchivePomodoroResponse>
{
    public Guid Id { get; set; }

    public long Version { get; set; }
}
