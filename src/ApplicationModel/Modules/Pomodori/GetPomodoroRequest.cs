namespace Pomodorium;

public class GetPomodoroRequest : Request<GetPomodoroResponse>
{
    public string Id { get; set; }
}
