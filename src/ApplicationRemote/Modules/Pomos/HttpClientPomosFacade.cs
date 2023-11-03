using MediatR;
using System.Net.Http.Json;
using System.Text.Json;

namespace Pomodorium.Modules.Pomos
{
    public class HttpClientPomosFacade :
        IRequestHandler<GetPomosRequest, GetPomosResponse>,
        IRequestHandler<GetPomodoroRequest, GetPomodoroResponse>,
        IRequestHandler<PostPomodoroRequest, PostPomodoroResponse>,
        IRequestHandler<PutPomodoroRequest, PutPomodoroResponse>,
        IRequestHandler<DeletePomodoroRequest, DeletePomodoroResponse>
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

        public async Task<PostPomodoroResponse> Handle(PostPomodoroRequest request, CancellationToken cancellationToken)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync("api/timers", request, cancellationToken);

            var response = await httpResponse.Content.ReadFromJsonAsync<PostPomodoroResponse>(JsonSerializerOptions.Default, cancellationToken);

            return response;
        }

        public async Task<PutPomodoroResponse> Handle(PutPomodoroRequest request, CancellationToken cancellationToken)
        {
            var httpResponse = await _httpClient.PutAsJsonAsync($"api/timers/{request.Id}", request, cancellationToken);

            var response = await httpResponse.Content.ReadFromJsonAsync<PutPomodoroResponse>(JsonSerializerOptions.Default, cancellationToken);

            return response;
        }

        public async Task<DeletePomodoroResponse> Handle(DeletePomodoroRequest request, CancellationToken cancellationToken)
        {
            var response = await _httpClient.DeleteFromJsonAsync<DeletePomodoroResponse>($"api/timers/{request.Id}?version={request.Version}", cancellationToken);

            return response;
        }
    }
}
