namespace Pomodorium.Modules.Timers;

public class GetTimersRequest : Request<GetTimersResponse>
{
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
}
