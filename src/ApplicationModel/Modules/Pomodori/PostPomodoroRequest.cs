namespace Pomodorium;

public class PostPomodoroRequest : Request
{
    public DateTime StartDateTime { get; set; }

    public string? Description { get; set; }
}
