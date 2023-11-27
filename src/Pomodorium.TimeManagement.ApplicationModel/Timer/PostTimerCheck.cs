namespace Pomodorium.TimeManagement.Timer;

public class PostTimerCheckRequest : Request<PostTimerCheckResponse>
{

}

public class PostTimerCheckResponse : Response
{
    public PostTimerCheckResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public PostTimerCheckResponse()
    {

    }
}
