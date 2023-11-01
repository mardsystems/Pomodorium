namespace Pomodorium.Events;

public class GetEventsRequest : Request<GetEventsResponse>
{
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
}
