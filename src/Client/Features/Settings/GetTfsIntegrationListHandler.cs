using MediatR;
using System.Net.Http.Json;
using System.Text.Json;

namespace Pomodorium.Features.Settings;

public class GetTfsIntegrationListHandler : IRequestHandler<GetTfsIntegrationListRequest, GetTfsIntegrationListResponse>
{
    private readonly HttpClient _httpClient;

    public GetTfsIntegrationListHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<GetTfsIntegrationListResponse> Handle(GetTfsIntegrationListRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetFromJsonAsync<GetTfsIntegrationListResponse>("api/Settings/TfsIntegration", JsonSerializerOptions.Default, cancellationToken);

        return response;
    }
}
