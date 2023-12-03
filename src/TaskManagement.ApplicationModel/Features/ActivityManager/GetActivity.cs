using Pomodorium.Enums;

namespace Pomodorium.Features.ActivityManager;

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

public class ActivityDetails
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
