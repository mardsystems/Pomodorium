namespace Pomodorium.Modules.Activities;

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
