namespace Pomodorium;

public class GetPomodoriRequest : Request
{
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
}
