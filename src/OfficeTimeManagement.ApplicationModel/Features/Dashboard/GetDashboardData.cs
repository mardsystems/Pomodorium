namespace Pomodorium.Features.Dashboard;

public class GetDashboardRequest : Request<GetDashboardResponse>
{

}

public class GetDashboardResponse : Response
{
    public GetDashboardResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public GetDashboardResponse() { }
}
