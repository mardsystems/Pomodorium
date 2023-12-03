using MediatR;
using System.Net.Http.Json;
using System.Text.Json;

namespace Pomodorium.Features.TaskSynchronizer;

public class SyncTasksFromTrelloHandler : IRequestHandler<SyncTasksFromTrelloRequest, SyncTasksFromTrelloResponse>
{
    private readonly HttpClient _httpClient;

    public SyncTasksFromTrelloHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<SyncTasksFromTrelloResponse> Handle(SyncTasksFromTrelloRequest request, CancellationToken cancellationToken)
    {
        var httpResponse = await _httpClient.PostAsJsonAsync("api/TaskSynchronizer/SyncTasksFromTrello", request, cancellationToken);

        var response = await httpResponse.Content.ReadFromJsonAsync<SyncTasksFromTrelloResponse>(JsonSerializerOptions.Default, cancellationToken);

        return response;
    }
}
