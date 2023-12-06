namespace Pomodorium.Features.FlowTimer;

public class FlowTimerRequestHandler :
    IRequestHandler<GetFlowsRequest, GetFlowsResponse>,
    IRequestHandler<GetFlowtimeRequest, GetFlowtimeResponse>,
    IRequestHandler<CreateFlowtimeRequest, CreateFlowtimeResponse>,
    IRequestHandler<StartFlowtimeRequest, StartFlowtimeResponse>,
    IRequestHandler<StartFlowtimeFromTaskRequest, StartFlowtimeFromTaskResponse>,
    IRequestHandler<InterruptFlowtimeRequest, InterruptFlowtimeResponse>,
    IRequestHandler<StopFlowtimeRequest, StopFlowtimeResponse>,
    IRequestHandler<ArchiveFlowtimeRequest, ArchiveFlowtimeResponse>
{
    private readonly FlowTimerClient _client;

    public FlowTimerRequestHandler(FlowTimerClient client)
    {
        _client = client;
    }

    public async Task<GetFlowsResponse> Handle(GetFlowsRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.GetFlowsAsync(request, cancellationToken);

        return response;
    }

    public async Task<GetFlowtimeResponse> Handle(GetFlowtimeRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.GetFlowtimeAsync(request, cancellationToken);

        return response;
    }

    public async Task<CreateFlowtimeResponse> Handle(CreateFlowtimeRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.CreateFlowtimeAsync(request, cancellationToken);

        return response;
    }

    public async Task<StartFlowtimeResponse> Handle(StartFlowtimeRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.StartFlowtimeAsync(request, cancellationToken);

        return response;
    }

    public async Task<StartFlowtimeFromTaskResponse> Handle(StartFlowtimeFromTaskRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.StartFlowtimeFromTaskAsync(request, cancellationToken);

        return response;
    }

    public async Task<InterruptFlowtimeResponse> Handle(InterruptFlowtimeRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.InterruptFlowtimeAsync(request, cancellationToken);

        return response;
    }

    public async Task<StopFlowtimeResponse> Handle(StopFlowtimeRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.StopFlowtimeAsync(request, cancellationToken);

        return response;
    }

    public async Task<ArchiveFlowtimeResponse> Handle(ArchiveFlowtimeRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.ArchiveFlowtimeAsync(request, cancellationToken);

        return response;
    }
}
