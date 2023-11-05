namespace Pomodorium.Timer;

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
