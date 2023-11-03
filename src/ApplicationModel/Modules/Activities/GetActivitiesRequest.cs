namespace Pomodorium.Modules.Activities;

public class GetActivitiesRequest : Request<GetActivitiesResponse>
{
    public int PageSize { get; set; }

    public int PageIndex { get; set; }
}
