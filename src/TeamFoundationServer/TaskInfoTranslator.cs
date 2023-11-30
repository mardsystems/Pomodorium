using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Pomodorium.Features.TaskSynchronizer;

namespace Pomodorium.TeamFoundationServer;

public static class TaskInfoTranslator
{
    public static TaskInfo ToTaskInfo(this WorkItem workItem)
    {
        return new TaskInfo
        {
            Reference = workItem.Id.ToString(),
            Name = $"{workItem.Fields["System.Title"]} (#{workItem.Id})"
        };
    }
}
