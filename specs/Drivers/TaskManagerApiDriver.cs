using Pomodorium.Features.TaskManager;
using Pomodorium.Support;

namespace Pomodorium.Drivers;

public class TaskManagerApiDriver
{
    private readonly WebApiContext _webApi;

    public ActionAttempt<TaskRegistrationRequest, TaskRegistrationResponse> CreateTaskAction { get; }

    public ActionAttempt<TaskDescriptionChangeRequest, TaskDescriptionChangeResponse> ChangeTaskDescriptionAction { get; }

    public ActionAttempt<TaskDetailsRequest, TaskDetailsResponse> GetTaskAction { get; }

    public ActionAttempt<TaskArchiveRequest, TaskArchiveResponse> ArchiveTaskAction { get; }

    public TaskManagerApiDriver(WebApiContext webApi, ActionAttemptFactory actionAttemptFactory)
    {
        _webApi = webApi;

        CreateTaskAction = actionAttemptFactory.CreateWithStatusCheck<TaskRegistrationRequest, TaskRegistrationResponse>(
            nameof(CreateTaskAction),
            request => _webApi.ExecutePost<TaskRegistrationResponse>("api/TaskManager/CreateTask", request));

        GetTaskAction = actionAttemptFactory.CreateWithStatusCheck<TaskDetailsRequest, TaskDetailsResponse>(
            nameof(GetTaskAction),
            request => _webApi.ExecutePost<TaskDetailsResponse>($"/api/TaskManager/GetTask", request));

        ChangeTaskDescriptionAction = actionAttemptFactory.CreateWithStatusCheck<TaskDescriptionChangeRequest, TaskDescriptionChangeResponse>(
            nameof(ChangeTaskDescriptionAction),
            request => _webApi.ExecutePost<TaskDescriptionChangeResponse>($"api/TaskManager/ChangeTaskDescription", request));

        ArchiveTaskAction = actionAttemptFactory.CreateWithStatusCheck<TaskArchiveRequest, TaskArchiveResponse>(
            nameof(ArchiveTaskAction),
            request => _webApi.ExecutePost<TaskArchiveResponse>($"api/TaskManager/ArchiveTask", request));
    }
}
