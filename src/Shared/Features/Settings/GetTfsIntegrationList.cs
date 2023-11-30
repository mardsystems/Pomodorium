using System.ApplicationModel;

namespace Pomodorium.Features.Settings;

public class GetTfsIntegrationListRequest : Request<GetTfsIntegrationListResponse>
{
    public int PageSize { get; set; }

    public int PageIndex { get; set; }
}

public class GetTfsIntegrationListResponse : Response
{
    public GetTfsIntegrationListResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public IEnumerable<TfsIntegration> TfsIntegrationList { get; set; }

    public GetTfsIntegrationListResponse() { }
}
