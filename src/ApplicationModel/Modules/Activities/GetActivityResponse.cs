namespace Pomodorium.Modules.Activities;

public class GetActivityResponse : Response
{
    public GetActivityResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public ActivityDetails ActivityDetails { get; set; }

    public GetActivityResponse()
    {
        
    }
}
