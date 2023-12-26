using Pomodorium.Features.TaskSynchronizer;
using Pomodorium.Support;

namespace Pomodorium.Drivers;

public class TaskSynchronizerApiDriver
{
    private readonly WebApiContext _webApi;

    public ActionAttempt<TaskSyncFromTfsRequest, TaskSyncFromTfsResponse> SynchTasksAction { get; }

    public TaskSynchronizerApiDriver(WebApiContext webApi, ActionAttemptFactory actionAttemptFactory)
    {
        _webApi = webApi;

        SynchTasksAction = actionAttemptFactory.CreateWithStatusCheck<TaskSyncFromTfsRequest, TaskSyncFromTfsResponse>(
            nameof(SynchTasksAction),
            request => _webApi.ExecutePost<TaskSyncFromTfsResponse>("api/TaskSynchronizer/TaskSyncFromTfs", request));
    }
}
