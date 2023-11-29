using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace Pomodorium.Trello;

public class ListsAdapter
{
    private readonly IHttpClientFactory _httpClientFactory;

    private readonly string _key;
    
    private readonly string _token;

    public ListsAdapter(
        IHttpClientFactory httpClientFactory,
        IOptions<TrelloOptions> optionsInterface)
    {
        _httpClientFactory = httpClientFactory;

        var options = optionsInterface.Value;

        _key = options.Key;

        _token = options.Token;
    }

    public async Task<IEnumerable<Card>> GetCards(string listId)
    {
        var httpClient = _httpClientFactory.CreateClient(TrelloOptions.NAME);

        var requestUri = $"1/lists/{listId}/cards?key={_key}&token={_token}";

        var cards = await httpClient.GetFromJsonAsync<Card[]>(requestUri, JsonSerializerOptions.Default).ConfigureAwait(false);

        return cards;
    }
}
