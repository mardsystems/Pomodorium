using MediatR;
using System.Net.Http.Json;
using System.Text.Json;

namespace TaskManagement.Features.TaskSynchronizer;

public class SyncTasksFromTfsHandler : IRequestHandler<TaskSyncFromTfsRequest, TaskSyncFromTfsResponse?>
{
    private readonly HttpClient _httpClient;

    public SyncTasksFromTfsHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TaskSyncFromTfsResponse?> Handle(TaskSyncFromTfsRequest request, CancellationToken cancellationToken)
    {
        var httpResponse = await _httpClient.PostAsJsonAsync("api/TaskSynchronizer/SyncTasksFromTfs", request, cancellationToken);

        var response = await httpResponse.Content.ReadFromJsonAsync<TaskSyncFromTfsResponse>(JsonSerializerOptions.Default, cancellationToken);

        return response;
    }
}
