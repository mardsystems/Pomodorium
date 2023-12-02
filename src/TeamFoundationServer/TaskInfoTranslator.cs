using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Pomodorium.Features.Settings;
using Pomodorium.TaskManagement.Model.Integrations;

namespace Pomodorium.TeamFoundationServer;

public static class TaskInfoTranslator
{
    public static TaskInfo ToTaskInfo(this WorkItem workItem, IntegrationBase integrationBase)
    {
        return new TaskInfo(
            IntegrationType.TFS,
            integrationBase.Id,
            integrationBase.Name,
            workItem.Id.ToString(),
            $"{workItem.Fields["System.Title"]} (#{workItem.Id})");
    }
}
