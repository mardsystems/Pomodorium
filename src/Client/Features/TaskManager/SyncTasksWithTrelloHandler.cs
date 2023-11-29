using MediatR;
using System.Net.Http.Json;
using System.Text.Json;

namespace Pomodorium.Features.TaskManager;

public class SyncTasksWithTrelloHandler : IRequestHandler<SyncTasksWithTrelloRequest, SyncTasksWithTrelloResponse>
{
    private readonly HttpClient _httpClient;

    public SyncTasksWithTrelloHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<SyncTasksWithTrelloResponse> Handle(SyncTasksWithTrelloRequest request, CancellationToken cancellationToken)
    {
        var httpResponse = await _httpClient.PostAsJsonAsync("api/taskmanager/syncTasksWithTrello", request, cancellationToken);

        var response = await httpResponse.Content.ReadFromJsonAsync<SyncTasksWithTrelloResponse>(JsonSerializerOptions.Default, cancellationToken);

        return response;
    }
}
