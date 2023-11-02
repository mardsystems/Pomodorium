namespace Pomodorium.Modules.Flows;

public class CreateFlowtimeRequest : Request<CreateFlowtimeResponse>
{
    public string? TaskDescription { get; set; }
}
