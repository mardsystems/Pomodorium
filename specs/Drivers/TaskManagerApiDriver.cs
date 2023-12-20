using Pomodorium.Features.TaskManager;
using Pomodorium.Support;

namespace Pomodorium.Drivers;

public class TaskManagerApiDriver
{
    private readonly WebApiContext _webApi;

    public ActionAttempt<CreateTaskRequest, CreateTaskResponse> CreateTaskAction { get; }

    public ActionAttempt<ChangeTaskDescriptionRequest, ChangeTaskDescriptionResponse> ChangeTaskDescriptionAction { get; }

    public ActionAttempt<GetTaskRequest, GetTaskResponse> GetTaskAction { get; }

    public ActionAttempt<ArchiveTaskRequest, ArchiveTaskResponse> ArchiveTaskAction { get; }

    public TaskManagerApiDriver(WebApiContext webApi, ActionAttemptFactory actionAttemptFactory)
    {
        _webApi = webApi;

        CreateTaskAction = actionAttemptFactory.CreateWithStatusCheck<CreateTaskRequest, CreateTaskResponse>(
            nameof(CreateTaskAction),
            request => _webApi.ExecutePost<CreateTaskResponse>("api/TaskManager/CreateTask", request));

        GetTaskAction = actionAttemptFactory.CreateWithStatusCheck<GetTaskRequest, GetTaskResponse>(
            nameof(GetTaskAction),
            request => _webApi.ExecutePost<GetTaskResponse>($"/api/TaskManager/GetTask", request));

        ChangeTaskDescriptionAction = actionAttemptFactory.CreateWithStatusCheck<ChangeTaskDescriptionRequest, ChangeTaskDescriptionResponse>(
            nameof(ChangeTaskDescriptionAction),
            request => _webApi.ExecutePost<ChangeTaskDescriptionResponse>($"api/TaskManager/ChangeTaskDescription", request));

        ArchiveTaskAction = actionAttemptFactory.CreateWithStatusCheck<ArchiveTaskRequest, ArchiveTaskResponse>(
            nameof(ArchiveTaskAction),
            request => _webApi.ExecutePost<ArchiveTaskResponse>($"api/TaskManager/ArchiveTask", request));
    }
}
