using Pomodorium.Models;

namespace Pomodorium.Models.Tasks.Integrations;

public interface ITrelloIntegrationService
{
    Task<IEnumerable<TaskInfo>> GetTaskInfoList(TrelloIntegration trelloIntegration);
}
