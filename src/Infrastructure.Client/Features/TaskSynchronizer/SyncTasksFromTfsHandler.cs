using MediatR;
using System.Net.Http.Json;
using System.Text.Json;

namespace Pomodorium.Features.TaskSynchronizer;

public class SyncTasksFromTfsHandler : IRequestHandler<SyncTasksFromTfsRequest, SyncTasksFromTfsResponse>
{
    private readonly HttpClient _httpClient;

    public SyncTasksFromTfsHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<SyncTasksFromTfsResponse> Handle(SyncTasksFromTfsRequest request, CancellationToken cancellationToken)
    {
        var httpResponse = await _httpClient.PostAsJsonAsync("api/TaskSynchronizer/SyncTasksFromTfs", request, cancellationToken);

        var response = await httpResponse.Content.ReadFromJsonAsync<SyncTasksFromTfsResponse>(JsonSerializerOptions.Default, cancellationToken);

        return response;
    }
}
