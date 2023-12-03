namespace Pomodorium.Features.FlowTimer;

public class StartFlowtimeFromTaskRequest : Request<StartFlowtimeFromTaskResponse>
{
    public Guid TaskId { get; set; }
}

public class StartFlowtimeFromTaskResponse : Response
{
    public StartFlowtimeFromTaskResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public StartFlowtimeFromTaskResponse() { }
}
