namespace Pomodorium.Features.PomodoroTimer;

public class PomodoroTimerRequestHandler :
    IRequestHandler<PomodoroQueryRequest, PomodoroQueryResponse>,
    IRequestHandler<PomodoroDetailsRequest, PomodoroDetailsResponse>,
    IRequestHandler<PomodoroCreationRequest, PomodoroCreationResponse>,
    IRequestHandler<PomodoroCheckingRequest, PomodoroCheckingResponse>,
    IRequestHandler<PomodoroTaskRefinementRequest, PomodoroTaskRefinementResponse>,
    IRequestHandler<PomodoroArchivingRequest, PomodoroArchivingResponse>
{
    private readonly PomodoroTimerClient _client;

    public PomodoroTimerRequestHandler(PomodoroTimerClient client)
    {
        _client = client;
    }

    public async Task<PomodoroQueryResponse> Handle(PomodoroQueryRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.GetPomodoroQueryAsync(request.PageSize,request.PageIndex, cancellationToken);

        return response;
    }

    public async Task<PomodoroDetailsResponse> Handle(PomodoroDetailsRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.GetPomodoroDetailsAsync(request.Id, cancellationToken);

        return response;
    }

    public async Task<PomodoroCreationResponse> Handle(PomodoroCreationRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.PostPomodoroCreationAsync(request, cancellationToken);

        return response;
    }

    public async Task<PomodoroCheckingResponse> Handle(PomodoroCheckingRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.PostPomodoroCheckingAsync(request, cancellationToken);

        return response;
    }

    public async Task<PomodoroTaskRefinementResponse> Handle(PomodoroTaskRefinementRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.PostPomodoroTaskRefinementAsync(request, cancellationToken);

        return response;
    }

    public async Task<PomodoroArchivingResponse> Handle(PomodoroArchivingRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.PostPomodoroArchivingAsync(request, cancellationToken);

        return response;
    }
}
