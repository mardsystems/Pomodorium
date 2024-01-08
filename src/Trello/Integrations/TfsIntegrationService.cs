using Pomodorium.Models;
using TaskManagement.Models.Integrations;

namespace Trello.Integrations;

public class TrelloIntegrationService : ITrelloIntegrationService
{
    private readonly CardAdapter _cardAdapter;

    public TrelloIntegrationService(CardAdapter cardAdapter)
    {
        _cardAdapter = cardAdapter;
    }

    public async Task<IEnumerable<TaskInfo>> GetTaskInfoList(TrelloIntegration tfsIntegration)
    {
        var taskInfoList = await _cardAdapter.GetTaskInfoList(tfsIntegration).ConfigureAwait(false);

        return taskInfoList;
    }
}
