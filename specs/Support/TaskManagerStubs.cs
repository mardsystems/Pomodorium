using TaskManagement.Features.TaskManager;

namespace Pomodorium.Support;

public static class TaskManagerStubs
{
    public static TaskRegistrationRequest CreateTask()
    {
        return new TaskRegistrationRequest
        {
            Description = "Default Task"
        };
    }
}
