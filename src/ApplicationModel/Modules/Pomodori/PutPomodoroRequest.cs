namespace Pomodorium;

public class PutPomodoroRequest : Request<PutPomodoroResponse>
{
    public string Id { get; set; }

    public string Description { get; set; }
}
