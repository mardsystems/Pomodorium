namespace Pomodorium.Modules.Pomos;

public class CreatePomodoroRequest : Request<CreatePomodoroResponse>
{
    public string? Task { get; set; }

    public TimeSpan Timer { get; set; }    
}
