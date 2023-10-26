namespace Pomodorium;

public class DeletePomodoroRequest : Request<DeletePomodoroResponse>
{
    public string Id { get; set; }

    public long Version { get; set; }
}
