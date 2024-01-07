using Pomodorium.Models;
using System.Net.Http.Json;
using System.Text.Json;
using TaskManagement.Models.Integrations;

namespace Pomodorium.Integrations.Trello;

public class CardAdapter
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CardAdapter(
        IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<TaskInfo>> GetTaskInfoList(TrelloIntegration trelloIntegration)
    {
        var httpClient = _httpClientFactory.CreateClient(TrelloIntegrationOptions.CONFIGURATION_SECTION_NAME);

        var boardsUri = $"1/boards/{trelloIntegration.BoardId}/lists?key={trelloIntegration.Key}&token={trelloIntegration.Token}";

        var lists = await httpClient.GetFromJsonAsync<List[]>(boardsUri, JsonSerializerOptions.Default).ConfigureAwait(false) ?? throw new InvalidOperationException();

        var taskInfoList = new List<TaskInfo>();

        foreach (var list in lists)
        {
            var requestUri = $"1/lists/{list.id}/cards?key={trelloIntegration.Key}&token={trelloIntegration.Token}";

            var cards = await httpClient.GetFromJsonAsync<Card[]>(requestUri, JsonSerializerOptions.Default).ConfigureAwait(false) ?? throw new InvalidOperationException();

            var taskInfoListBuffer = cards.Select(x => x.ToTaskInfo(trelloIntegration));

            taskInfoList.AddRange(taskInfoListBuffer);
        }

        return taskInfoList;
    }
}
