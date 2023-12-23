using System.ApplicationModel;

namespace Pomodorium.Features.Maintenance;

public record IndexRebuildRequest : Request<IndexRebuildResponse>
{

}

public record IndexRebuildResponse(Guid CorrelationId) : Response(CorrelationId)
{

}
