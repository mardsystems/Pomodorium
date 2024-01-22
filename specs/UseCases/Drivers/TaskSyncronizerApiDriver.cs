using TaskManagement.Features.TaskSynchronizer;
using Pomodorium.Models;
using Pomodorium.Support;

namespace Pomodorium.Drivers;

public class TaskSynchronizerApiDriver
{
    private readonly WebApiContext _webApi;

    public ActionAttempt<TfsIntegration, TfsIntegration> PostTfsIntegrationAction { get; }

    public ActionAttempt<TaskSyncFromTfsRequest, TaskSyncFromTfsResponse> SynchTasksAction { get; }

    public TaskSynchronizerApiDriver(WebApiContext webApi, ActionAttemptFactory actionAttemptFactory)
    {
        _webApi = webApi;

        PostTfsIntegrationAction = actionAttemptFactory.CreateWithStatusCheck<TfsIntegration, TfsIntegration>(
            nameof(PostTfsIntegrationAction),
            request => _webApi.ExecutePost<TfsIntegration>("api/Settings/TfsIntegration", request), System.Net.HttpStatusCode.Created);

        SynchTasksAction = actionAttemptFactory.CreateWithStatusCheck<TaskSyncFromTfsRequest, TaskSyncFromTfsResponse>(
            nameof(SynchTasksAction),
            request => _webApi.ExecutePost<TaskSyncFromTfsResponse>("api/TaskSynchronizer/TaskSyncFromTfs", request));
    }
}
