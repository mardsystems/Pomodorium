namespace Pomodorium.Modules.Flows;

public class GetTaskResponse : Response
{
    public GetTaskResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public TaskDetails TaskDetails { get; set; }

    public GetTaskResponse()
    {
        
    }
}
