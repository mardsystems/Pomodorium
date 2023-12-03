using Pomodorium.Enums;

namespace Pomodorium.Features.ActivityManager;

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

public class ActivityQueryItem
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public DateTime? StartDateTime { get; set; }

    public DateTime? StopDateTime { get; set; }

    public ActivityStateEnum State { get; set; }

    public TimeSpan? Duration { get; set; }

    public string Description { get; set; }

    public long Version { get; set; }
}
