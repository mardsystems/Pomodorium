using System.Net.Http.Json;
using System.Text.Json;

namespace Pomodorium.Features.PomodoroTimer;

public class CreatePomodoroHandler : IRequestHandler<CreatePomodoroRequest, CreatePomodoroResponse>
{
    private readonly HttpClient _httpClient;

    public CreatePomodoroHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CreatePomodoroResponse> Handle(CreatePomodoroRequest request, CancellationToken cancellationToken)
    {
        var httpResponse = await _httpClient.PostAsJsonAsync("api/PomodoroTimer/pomos", request, cancellationToken);

        var response = await httpResponse.Content.ReadFromJsonAsync<CreatePomodoroResponse>(JsonSerializerOptions.Default, cancellationToken);

        return response;
    }
}