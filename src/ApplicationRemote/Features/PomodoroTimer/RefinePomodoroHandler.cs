using System.Net.Http.Json;
using System.Text.Json;

namespace Pomodorium.Features.PomodoroTimer;

public class RefinePomodoroHandler :    IRequestHandler<RefinePomodoroTaskRequest, RefinePomodoroTaskResponse>
{
    private readonly HttpClient _httpClient;

    public RefinePomodoroHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<RefinePomodoroTaskResponse> Handle(RefinePomodoroTaskRequest request, CancellationToken cancellationToken)
    {
        var httpResponse = await _httpClient.PutAsJsonAsync($"api/PomodoroTimer/pomos/{request.Id}", request, cancellationToken);

        var response = await httpResponse.Content.ReadFromJsonAsync<RefinePomodoroTaskResponse>(JsonSerializerOptions.Default, cancellationToken);

        return response;
    }
}