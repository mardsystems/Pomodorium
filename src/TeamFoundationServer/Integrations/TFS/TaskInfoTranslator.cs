using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Pomodorium.Enums;
using Pomodorium.Models;
using Pomodorium.Models.TaskManagement.Integrations;

namespace Pomodorium.Integrations.TFS;

public static class TaskInfoTranslator
{
    public static TaskInfo ToTaskInfo(this WorkItem workItem, IntegrationBase integrationBase)
    {
        //if (workItem == null)
        //{
        //    throw new ArgumentNullException(nameof(workItem));
        //}

        if (integrationBase.Id == null)
        {
            throw new InvalidOperationException();
        }

        if (integrationBase.Name == null)
        {
            throw new InvalidOperationException();
        }

        if (workItem.Id == null)
        {
            throw new InvalidOperationException();
        }

        var workItemId = workItem.Id.ToString() ?? throw new InvalidOperationException();

        return new TaskInfo(
            IntegrationTypeEnum.TFS,
            integrationBase.Id.Value,
            integrationBase.Name,
            workItemId,
            $"{workItem.Fields["System.Title"]} (#{workItem.Id})");
    }
}
