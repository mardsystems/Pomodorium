using MediatR;
using System.Net.Http.Json;
using System.Text.Json;

namespace Pomodorium.Features.TaskSynchronizer;

public class CreateTfsIntegrationHandler : IRequestHandler<CreateTfsIntegrationRequest, CreateTfsIntegrationResponse>
{
    private readonly HttpClient _httpClient;

    public CreateTfsIntegrationHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CreateTfsIntegrationResponse> Handle(CreateTfsIntegrationRequest request, CancellationToken cancellationToken)
    {
        var httpResponse = await _httpClient.PostAsJsonAsync("api/TaskSynchronizer/TfsIntegration/Create", request, cancellationToken);

        var response = await httpResponse.Content.ReadFromJsonAsync<CreateTfsIntegrationResponse>(JsonSerializerOptions.Default, cancellationToken);

        return response;
    }
}
