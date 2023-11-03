namespace Pomodorium.Modules.Pomos;

public class RefinePomodoroTaskRequest : Request<RefinePomodoroTaskResponse>
{
    public Guid Id { get; set; }

    public string Task { get; set; }

    public long Version { get; set; }
}
