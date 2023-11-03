namespace Pomodorium.Modules.Flows;

public class GetFlowtimeResponse : Response
{
    public GetFlowtimeResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public FlowtimeDetails FlowtimeDetails { get; set; }

    public GetFlowtimeResponse()
    {
        
    }
}
