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
        var response = await _client.GetPomosAsync(request, cancellationToken);

        return response;
    }

    public async Task<PomodoroDetailsResponse> Handle(PomodoroDetailsRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.GetPomodoroAsync(request, cancellationToken);

        return response;
    }

    public async Task<PomodoroCreationResponse> Handle(PomodoroCreationRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.CreatePomodoroAsync(request, cancellationToken);

        return response;
    }

    public async Task<PomodoroCheckingResponse> Handle(PomodoroCheckingRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.CheckPomodoroAsync(request, cancellationToken);

        return response;
    }

    public async Task<PomodoroTaskRefinementResponse> Handle(PomodoroTaskRefinementRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.RefinePomodoroTaskAsync(request, cancellationToken);

        return response;
    }

    public async Task<PomodoroArchivingResponse> Handle(PomodoroArchivingRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.ArchivePomodoroAsync(request, cancellationToken);

        return response;
    }
}
