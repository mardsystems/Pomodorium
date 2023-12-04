using Pomodorium.Features.TaskManager;
using Pomodorium.Support;

namespace Pomodorium.Drivers;

public class TaskManagerApiDriver
{
    private readonly WebApiContext _webApi;

    public ActionAttempt<CreateTaskRequest, CreateTaskResponse> CreateTaskAction { get; }

    public ActionAttempt<ChangeTaskDescriptionRequest, ChangeTaskDescriptionResponse> ChangeTaskDescriptionAction { get; }

    public ActionAttempt<GetTasksRequest, GetTasksResponse> GetTasksAction { get; }

    public ActionAttempt<ArchiveTaskRequest, ArchiveTaskResponse> ArchiveTaskAction { get; }

    public TaskManagerApiDriver(WebApiContext webApi, ActionAttemptFactory actionAttemptFactory)
    {
        _webApi = webApi;

        CreateTaskAction = actionAttemptFactory.CreateWithStatusCheck<CreateTaskRequest, CreateTaskResponse>(
            nameof(CreateTaskAction),
            request => _webApi.ExecutePost<CreateTaskResponse>("api/TaskManager/Tasks/Create", request));

        GetTasksAction = actionAttemptFactory.CreateWithStatusCheck<GetTasksRequest, GetTasksResponse>(
            nameof(GetTasksAction),
            request => _webApi.ExecuteGet<GetTasksResponse>($"/api/TaskManager/Tasks?description={request.Description}"));

        ChangeTaskDescriptionAction = actionAttemptFactory.CreateWithStatusCheck<ChangeTaskDescriptionRequest, ChangeTaskDescriptionResponse>(
            nameof(ChangeTaskDescriptionAction),
            request => _webApi.ExecutePost<ChangeTaskDescriptionResponse>($"api/TaskManager/Tasks/{request.TaskId}/Description/Change", request));

        ArchiveTaskAction = actionAttemptFactory.CreateWithStatusCheck<ArchiveTaskRequest, ArchiveTaskResponse>(
            nameof(ArchiveTaskAction),
            request => _webApi.ExecutePost<ArchiveTaskResponse>($"api/TaskManager/Tasks/{request.TaskId}/Archive", request));
    }

    public GetTasksResponse GetTasks(GetTasksRequest request)
    {
        return _webApi.ExecuteGet<GetTasksResponse>($"/api/TaskManager/Tasks?description={request.Description}").ResponseData;
    }

    public CreateTaskResponse CreateTask(CreateTaskRequest request)
    {
        return _webApi.ExecutePost<CreateTaskResponse>($"/api/TaskManager/Tasks/Create", request).ResponseData;
    }

    public GetTaskResponse GetTask(Guid taskId)
    {
        return _webApi.ExecuteGet<GetTaskResponse>($"/api/TaskManager/Tasks/{taskId}").ResponseData;
    }
}
