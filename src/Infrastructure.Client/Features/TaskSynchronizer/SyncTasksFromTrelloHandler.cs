using MediatR;
using System.Net.Http.Json;
using System.Text.Json;

namespace Pomodorium.Features.TaskSynchronizer;

public class SyncTasksFromTrelloHandler : IRequestHandler<TaskSyncFromTrelloRequest, TaskSyncFromTrelloResponse?>
{
    private readonly HttpClient _httpClient;

    public SyncTasksFromTrelloHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TaskSyncFromTrelloResponse?> Handle(TaskSyncFromTrelloRequest request, CancellationToken cancellationToken)
    {
        var httpResponse = await _httpClient.PostAsJsonAsync("api/TaskSynchronizer/SyncTasksFromTrello", request, cancellationToken);

        var response = await httpResponse.Content.ReadFromJsonAsync<TaskSyncFromTrelloResponse>(JsonSerializerOptions.Default, cancellationToken);

        return response;
    }
}
