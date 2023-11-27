namespace Pomodorium.TaskManagement.ActivityManager;

public class GetActivitiesRequest : Request<GetActivitiesResponse>
{
    public int PageSize { get; set; }

    public int PageIndex { get; set; }
}

public class GetActivitiesResponse : Response
{
    public GetActivitiesResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public IEnumerable<ActivityQueryItem> ActivityQueryItems { get; set; }

    public GetActivitiesResponse()
    {

    }
}
