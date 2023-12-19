namespace Pomodorium.Features.Timer;

public record CheckTimerRequest : Request<CheckTimerResponse>
{

}

public record CheckTimerResponse(Guid CorrelationId) : Response(CorrelationId)
{

}
