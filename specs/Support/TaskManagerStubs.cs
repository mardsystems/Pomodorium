using Pomodorium.Features.TaskManager;

namespace Pomodorium.Support;

public static class TaskManagerStubs
{
    public static CreateTaskRequest CreateTask()
    {
        return new CreateTaskRequest()
        {
            Description = "Default Task"
        };
    }

}
