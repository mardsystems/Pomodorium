using TaskManagement.Features.TaskManager;
using Pomodorium.Support;

namespace Pomodorium.Drivers;

public class TaskManagerApiDriver
{
    private readonly WebApiContext _webApi;

    public ActionAttempt<TaskRegistrationRequest, TaskRegistrationResponse> CreateTaskAction { get; }

    public ActionAttempt<TaskDescriptionChangeRequest, TaskDescriptionChangeResponse> ChangeTaskDescriptionAction { get; }

    public ActionAttempt<TaskQueryRequest, TaskQueryResponse> QueryTasksAction { get; }

    public ActionAttempt<TaskDetailsRequest, TaskDetailsResponse> GetTaskAction { get; }

    public ActionAttempt<TaskArchivingRequest, TaskArchivingResponse> ArchiveTaskAction { get; }

    public TaskManagerApiDriver(WebApiContext webApi, ActionAttemptFactory actionAttemptFactory)
    {
        _webApi = webApi;

        CreateTaskAction = actionAttemptFactory.CreateWithStatusCheck<TaskRegistrationRequest, TaskRegistrationResponse>(
            nameof(CreateTaskAction),
            request => _webApi.ExecutePost<TaskRegistrationResponse>("api/TaskManager/TaskRegistration", request));

        QueryTasksAction = actionAttemptFactory.CreateWithStatusCheck<TaskQueryRequest, TaskQueryResponse>(
            nameof(QueryTasksAction),
            request => _webApi.ExecuteGet<TaskQueryResponse>($"/api/TaskManager/TaskQuery"));

        GetTaskAction = actionAttemptFactory.CreateWithStatusCheck<TaskDetailsRequest, TaskDetailsResponse>(
            nameof(GetTaskAction),
            request => _webApi.ExecuteGet<TaskDetailsResponse>($"/api/TaskManager/TaskDetails?TaskId={request.TaskId}"));

        ChangeTaskDescriptionAction = actionAttemptFactory.CreateWithStatusCheck<TaskDescriptionChangeRequest, TaskDescriptionChangeResponse>(
            nameof(ChangeTaskDescriptionAction),
            request => _webApi.ExecutePost<TaskDescriptionChangeResponse>($"api/TaskManager/TaskDescriptionChange", request));

        ArchiveTaskAction = actionAttemptFactory.CreateWithStatusCheck<TaskArchivingRequest, TaskArchivingResponse>(
            nameof(ArchiveTaskAction),
            request => _webApi.ExecutePost<TaskArchivingResponse>($"api/TaskManager/TaskArchiving", request));
    }
}
