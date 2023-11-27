namespace Pomodorium.TaskManagement.ActivityManager;

public class GetActivityRequest : Request<GetActivityResponse>
{
    public Guid Id { get; set; }
}

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
