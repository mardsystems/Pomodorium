namespace Pomodorium.TimeManagement.FlowTimer;

public class GetFlowtimeRequest : Request<GetFlowtimeResponse>
{
    public Guid Id { get; set; }
}

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
