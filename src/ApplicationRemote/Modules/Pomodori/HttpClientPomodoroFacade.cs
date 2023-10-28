using MediatR;
using System.Net.Http.Json;
using System.Text.Json;

namespace Pomodorium.Modules.Pomodori
{
    public class HttpClientPomodoroFacade :
        IRequestHandler<GetPomodoriRequest, GetPomodoriResponse>,
        IRequestHandler<GetPomodoroRequest, GetPomodoroResponse>,
        IRequestHandler<PostPomodoroRequest, PostPomodoroResponse>,
        IRequestHandler<PutPomodoroRequest, PutPomodoroResponse>,
        IRequestHandler<DeletePomodoroRequest, DeletePomodoroResponse>
    {
        private readonly HttpClient _httpClient;

        public HttpClientPomodoroFacade(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GetPomodoriResponse> Handle(GetPomodoriRequest request, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetFromJsonAsync<GetPomodoriResponse>("api/pomodori", cancellationToken);

            return response;
        }

        public async Task<GetPomodoroResponse> Handle(GetPomodoroRequest request, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetFromJsonAsync<GetPomodoroResponse>($"api/pomodori/{request.Id}", cancellationToken);

            return response;
        }

        public async Task<PostPomodoroResponse> Handle(PostPomodoroRequest request, CancellationToken cancellationToken)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync("api/pomodori", request, cancellationToken);

            var response = await httpResponse.Content.ReadFromJsonAsync<PostPomodoroResponse>(JsonSerializerOptions.Default, cancellationToken);

            return response;
        }

        public async Task<PutPomodoroResponse> Handle(PutPomodoroRequest request, CancellationToken cancellationToken)
        {
            var httpResponse = await _httpClient.PutAsJsonAsync($"api/pomodori/{request.Id}", request, cancellationToken);

            var response = await httpResponse.Content.ReadFromJsonAsync<PutPomodoroResponse>(JsonSerializerOptions.Default, cancellationToken);

            return response;
        }

        public async Task<DeletePomodoroResponse> Handle(DeletePomodoroRequest request, CancellationToken cancellationToken)
        {
            var response = await _httpClient.DeleteFromJsonAsync<DeletePomodoroResponse>($"api/pomodori/{request.Id}?version={request.Version}", cancellationToken);

            return response;
        }
    }
}
