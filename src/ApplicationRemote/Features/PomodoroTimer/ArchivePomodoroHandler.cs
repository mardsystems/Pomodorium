using System.Net.Http.Json;

namespace Pomodorium.Features.PomodoroTimer;

public class ArchivePomodoroHandler : IRequestHandler<ArchivePomodoroRequest, ArchivePomodoroResponse>
{
    private readonly HttpClient _httpClient;

    public ArchivePomodoroHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ArchivePomodoroResponse> Handle(ArchivePomodoroRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.DeleteFromJsonAsync<ArchivePomodoroResponse>($"api/PomodoroTimer/pomos/{request.Id}?version={request.Version}", cancellationToken);

        return response;
    }
}