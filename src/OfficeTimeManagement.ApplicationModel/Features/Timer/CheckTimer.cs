namespace Pomodorium.Features.Timer;

public class CheckTimerRequest : Request<CheckTimerResponse>
{

}

public class CheckTimerResponse : Response
{
    public CheckTimerResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public CheckTimerResponse()
    {

    }
}
