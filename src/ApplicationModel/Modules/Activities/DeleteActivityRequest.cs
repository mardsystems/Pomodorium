namespace Pomodorium.Modules.Activities;

public class DeleteActivityRequest : Request<DeleteActivityResponse>
{
    public Guid Id { get; set; }

    public long Version { get; set; }
}
