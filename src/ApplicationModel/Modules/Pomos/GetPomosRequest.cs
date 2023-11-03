namespace Pomodorium.Modules.Pomos;

public class GetPomosRequest : Request<GetPomosResponse>
{
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
}
