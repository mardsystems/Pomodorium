using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Pomodorium.Enums;
using Pomodorium.Models;
using Pomodorium.TaskManagement.Model.Integrations;

namespace Pomodorium.TeamFoundationServer;

public static class TaskInfoTranslator
{
    public static TaskInfo ToTaskInfo(this WorkItem workItem, IntegrationBase integrationBase)
    {
        return new TaskInfo(
            IntegrationTypeEnum.TFS,
            integrationBase.Id.Value,
            integrationBase.Name,
            workItem.Id.ToString(),
            $"{workItem.Fields["System.Title"]} (#{workItem.Id})");
    }
}
