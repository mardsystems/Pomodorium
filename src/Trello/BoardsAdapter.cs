using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace Pomodorium.Trello;

public class BoardsAdapter
{
    private readonly IHttpClientFactory _httpClientFactory;

    private readonly string _key;

    private readonly string _token;

    public BoardsAdapter(
        IHttpClientFactory httpClientFactory,
        IOptions<TrelloOptions> optionsInterface)
    {
        _httpClientFactory = httpClientFactory;

        var options = optionsInterface.Value;

        _key = options.Key;

        _token = options.Token;
    }

    public async Task<IEnumerable<List>> GetLists(string boardId)
    {
        var httpClient = _httpClientFactory.CreateClient(TrelloOptions.NAME);

        var requestUri = $"1/boards/{boardId}/lists?key={_key}&token={_token}";

        var lists = await httpClient.GetFromJsonAsync<List[]>(requestUri, JsonSerializerOptions.Default).ConfigureAwait(false);

        return lists;
    }
}
