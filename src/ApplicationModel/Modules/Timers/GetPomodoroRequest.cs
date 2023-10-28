namespace Pomodorium;

public class GetPomodoroRequest : Request<GetPomodoroResponse>
{
    public Guid Id { get; set; }
}
