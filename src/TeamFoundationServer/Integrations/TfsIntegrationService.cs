using Pomodorium.Models;
using Pomodorium.Models.Tasks.Integrations;

namespace TeamFoundationServer.Integrations;

public class TfsIntegrationService : ITfsIntegrationService
{
    private readonly WorkItemAdapter _workItemAdapter;

    public TfsIntegrationService(WorkItemAdapter workItemAdapter)
    {
        _workItemAdapter = workItemAdapter;
    }

    public async Task<IEnumerable<TaskInfo>> GetTaskInfoList(TfsIntegration tfsIntegration)
    {
        var taskInfoList = await _workItemAdapter.GetTaskInfoList(tfsIntegration).ConfigureAwait(false);

        return taskInfoList;
    }
}
