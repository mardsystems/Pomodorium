namespace Pomodorium.Features.FlowTimer;

public class FlowTimerRequestHandler :
    IRequestHandler<FlowtimeQueryRequest, FlowtimeQueryResponse>,
    IRequestHandler<FlowtimeDetailsRequest, FlowtimeDetailsResponse>,
    IRequestHandler<FlowtimeCreationRequest, FlowtimeCreationResponse>,
    IRequestHandler<FlowtimeStartRequest, FlowtimeStartResponse>,
    IRequestHandler<FlowtimeStartFromTaskRequest, FlowtimeStartFromTaskResponse>,
    IRequestHandler<FlowtimeInterruptionRequest, FlowtimeInterruptionResponse>,
    IRequestHandler<FlowtimeStopRequest, FlowtimeStopResponse>,
    IRequestHandler<FlowtimeArchivingRequest, FlowtimeArchivingResponse>
{
    private readonly FlowTimerClient _client;

    public FlowTimerRequestHandler(FlowTimerClient client)
    {
        _client = client;
    }

    public async Task<FlowtimeQueryResponse> Handle(FlowtimeQueryRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.GetFlowsAsync(request, cancellationToken);

        return response;
    }

    public async Task<FlowtimeDetailsResponse> Handle(FlowtimeDetailsRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.GetFlowtimeAsync(request, cancellationToken);

        return response;
    }

    public async Task<FlowtimeCreationResponse> Handle(FlowtimeCreationRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.CreateFlowtimeAsync(request, cancellationToken);

        return response;
    }

    public async Task<FlowtimeStartResponse> Handle(FlowtimeStartRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.StartFlowtimeAsync(request, cancellationToken);

        return response;
    }

    public async Task<FlowtimeStartFromTaskResponse> Handle(FlowtimeStartFromTaskRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.StartFlowtimeFromTaskAsync(request, cancellationToken);

        return response;
    }

    public async Task<FlowtimeInterruptionResponse> Handle(FlowtimeInterruptionRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.InterruptFlowtimeAsync(request, cancellationToken);

        return response;
    }

    public async Task<FlowtimeStopResponse> Handle(FlowtimeStopRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.StopFlowtimeAsync(request, cancellationToken);

        return response;
    }

    public async Task<FlowtimeArchivingResponse> Handle(FlowtimeArchivingRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.ArchiveFlowtimeAsync(request, cancellationToken);

        return response;
    }
}
