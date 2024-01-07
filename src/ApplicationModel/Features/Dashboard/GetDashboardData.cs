namespace Pomodorium.Features.Dashboard;

public record GetDashboardRequest : Request<GetDashboardResponse>
{

}

public record GetDashboardResponse(Guid CorrelationId) : Response(CorrelationId)
{

}
