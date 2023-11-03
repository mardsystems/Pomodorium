namespace Pomodorium.Modules.Pomos;

public class PostPomodoroRequest : Request<PostPomodoroResponse>
{
    public DateTime StartDateTime { get; set; }

    public string? Description { get; set; }
}
