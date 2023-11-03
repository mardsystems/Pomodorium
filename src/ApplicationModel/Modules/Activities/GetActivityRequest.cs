namespace Pomodorium.Modules.Activities;

public class GetActivityRequest : Request<GetActivityResponse>
{
    public Guid Id { get; set; }
}
