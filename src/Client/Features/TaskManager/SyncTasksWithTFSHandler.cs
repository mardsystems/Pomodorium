using MediatR;
using System.Net.Http.Json;
using System.Text.Json;

namespace Pomodorium.Features.TaskManager;

public class SyncTasksWithTFSHandler : IRequestHandler<SyncTasksWithTFSRequest, SyncTasksWithTFSResponse>
{
    private readonly HttpClient _httpClient;

    public SyncTasksWithTFSHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<SyncTasksWithTFSResponse> Handle(SyncTasksWithTFSRequest request, CancellationToken cancellationToken)
    {
        var httpResponse = await _httpClient.PostAsJsonAsync("api/taskmanager/syncTasksWithTFS", request, cancellationToken);

        var response = await httpResponse.Content.ReadFromJsonAsync<SyncTasksWithTFSResponse>(JsonSerializerOptions.Default, cancellationToken);

        return response;
    }
}
