namespace Pomodorium;

public class GetPomodoriRequest : Request<GetPomodoriResponse>
{
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
}
