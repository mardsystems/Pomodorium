namespace Pomodorium.Modules.Timers;

public class DeletePomodoroRequest : Request<DeletePomodoroResponse>
{
    public Guid Id { get; set; }

    public long Version { get; set; }
}
