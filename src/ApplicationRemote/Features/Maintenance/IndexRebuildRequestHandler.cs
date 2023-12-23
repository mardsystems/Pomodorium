namespace Pomodorium.Features.Maintenance;

public class IndexRebuildRequestHandler :
    IRequestHandler<IndexRebuildRequest, IndexRebuildResponse>
{
    private readonly MaintenanceClient _client;

    public IndexRebuildRequestHandler(MaintenanceClient client)
    {
        _client = client;
    }

    public async Task<IndexRebuildResponse> Handle(IndexRebuildRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.PostIndexRebuildAsync(request, cancellationToken);

        return response;
    }
}
