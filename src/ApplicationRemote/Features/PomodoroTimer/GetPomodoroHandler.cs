using System.Net.Http.Json;

namespace Pomodorium.Features.PomodoroTimer;

public class GetPomodoroHandler : IRequestHandler<GetPomodoroRequest, GetPomodoroResponse>
{
    private readonly HttpClient _httpClient;

    public GetPomodoroHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<GetPomodoroResponse> Handle(GetPomodoroRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetFromJsonAsync<GetPomodoroResponse>($"api/PomodoroTimer/pomos/{request.Id}", cancellationToken);

        return response;
    }
}