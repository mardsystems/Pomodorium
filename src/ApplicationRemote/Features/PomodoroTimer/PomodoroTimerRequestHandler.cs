namespace Pomodorium.Features.PomodoroTimer;

public class PomodoroTimerRequestHandler :
    IRequestHandler<GetPomosRequest, GetPomosResponse>,
    IRequestHandler<GetPomodoroRequest, GetPomodoroResponse>,
    IRequestHandler<CreatePomodoroRequest, CreatePomodoroResponse>,
    IRequestHandler<CheckPomodoroRequest, CheckPomodoroResponse>,
    IRequestHandler<RefinePomodoroTaskRequest, RefinePomodoroTaskResponse>,
    IRequestHandler<ArchivePomodoroRequest, ArchivePomodoroResponse>
{
    private readonly PomodoroTimerClient _client;

    public PomodoroTimerRequestHandler(PomodoroTimerClient client)
    {
        _client = client;
    }

    public async Task<GetPomosResponse> Handle(GetPomosRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.GetPomosAsync(request, cancellationToken);

        return response;
    }

    public async Task<GetPomodoroResponse> Handle(GetPomodoroRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.GetPomodoroAsync(request, cancellationToken);

        return response;
    }

    public async Task<CreatePomodoroResponse> Handle(CreatePomodoroRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.CreatePomodoroAsync(request, cancellationToken);

        return response;
    }

    public async Task<CheckPomodoroResponse> Handle(CheckPomodoroRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.CheckPomodoroAsync(request, cancellationToken);

        return response;
    }

    public async Task<RefinePomodoroTaskResponse> Handle(RefinePomodoroTaskRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.RefinePomodoroTaskAsync(request, cancellationToken);

        return response;
    }

    public async Task<ArchivePomodoroResponse> Handle(ArchivePomodoroRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.ArchivePomodoroAsync(request, cancellationToken);

        return response;
    }
}
