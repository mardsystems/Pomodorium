namespace Pomodorium.Modules.Flows;

public class StartFlowtimeRequest : Request<StartFlowtimeResponse>
{
    public Guid Id { get; set; }

    public DateTime StartDateTime { get; set; }

    public long Version { get; set; }
}
