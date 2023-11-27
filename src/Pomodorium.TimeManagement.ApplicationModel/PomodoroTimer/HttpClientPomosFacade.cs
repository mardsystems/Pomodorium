using System.Net.Http.Json;
using System.Text.Json;

namespace Pomodorium.TimeManagement.PomodoroTimer;

public class HttpClientPomosFacade :
    IRequestHandler<GetPomosRequest, GetPomosResponse>,
    IRequestHandler<GetPomodoroRequest, GetPomodoroResponse>,
    IRequestHandler<CreatePomodoroRequest, CreatePomodoroResponse>,
    IRequestHandler<RefinePomodoroTaskRequest, RefinePomodoroTaskResponse>,
    IRequestHandler<ArchivePomodoroRequest, ArchivePomodoroResponse>
{
    private readonly HttpClient _httpClient;

    public HttpClientPomosFacade(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<GetPomosResponse> Handle(GetPomosRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetFromJsonAsync<GetPomosResponse>("api/timers", cancellationToken);

        return response;
    }

    public async Task<GetPomodoroResponse> Handle(GetPomodoroRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetFromJsonAsync<GetPomodoroResponse>($"api/timers/{request.Id}", cancellationToken);

        return response;
    }

    public async Task<CreatePomodoroResponse> Handle(CreatePomodoroRequest request, CancellationToken cancellationToken)
    {
        var httpResponse = await _httpClient.PostAsJsonAsync("api/timers", request, cancellationToken);

        var response = await httpResponse.Content.ReadFromJsonAsync<CreatePomodoroResponse>(JsonSerializerOptions.Default, cancellationToken);

        return response;
    }

    public async Task<RefinePomodoroTaskResponse> Handle(RefinePomodoroTaskRequest request, CancellationToken cancellationToken)
    {
        var httpResponse = await _httpClient.PutAsJsonAsync($"api/timers/{request.Id}", request, cancellationToken);

        var response = await httpResponse.Content.ReadFromJsonAsync<RefinePomodoroTaskResponse>(JsonSerializerOptions.Default, cancellationToken);

        return response;
    }

    public async Task<ArchivePomodoroResponse> Handle(ArchivePomodoroRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.DeleteFromJsonAsync<ArchivePomodoroResponse>($"api/timers/{request.Id}?version={request.Version}", cancellationToken);

        return response;
    }
}