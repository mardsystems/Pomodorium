using System.Net.Http.Json;

namespace Pomodorium.Features.PomodoroTimer;

public class GetPomosHandler : IRequestHandler<GetPomosRequest, GetPomosResponse>
{
    private readonly HttpClient _httpClient;

    public GetPomosHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<GetPomosResponse> Handle(GetPomosRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetFromJsonAsync<GetPomosResponse>("api/PomodoroTimer/pomos", cancellationToken);

        return response;
    }
}