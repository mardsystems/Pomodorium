namespace Pomodorium.Modules.Timers;

public class PostPomodoroRequest : Request<PostPomodoroResponse>
{
    public DateTime StartDateTime { get; set; }

    public string? Description { get; set; }
}
