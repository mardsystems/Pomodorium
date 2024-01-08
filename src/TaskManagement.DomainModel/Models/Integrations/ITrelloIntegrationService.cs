using Pomodorium.Models;

namespace TaskManagement.Models.Integrations;

public interface ITrelloIntegrationService
{
    Task<IEnumerable<TaskInfo>> GetTaskInfoList(TrelloIntegration trelloIntegration);
}
