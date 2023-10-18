namespace Pomodorium;

public class DeletePomodoroRequest : Request<DeletePomodoroResponse>
{
    public string Id { get; set; }
}
