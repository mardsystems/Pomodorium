namespace Pomodorium.Modules.Flows;

public class GetFlowtimeRequest : Request<GetFlowtimeResponse>
{
    public Guid Id { get; set; }
}
