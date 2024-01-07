using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Pomodorium.Enums;
using Pomodorium.Models;
using TaskManagement.Models.Integrations;

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
            throw new NullReferenceException("Error on translate work item to task info: integrationBase.Id is required.");
        }

        if (integrationBase.Name == null)
        {
            throw new NullReferenceException("Error on translate work item to task info: integrationBase.Name is required.");
        }

        if (workItem.Id == null)
        {
            throw new NullReferenceException("Error on translate work item to task info: workItem.Id is required.");
        }

        var workItemId = workItem.Id.ToString() ?? throw new NullReferenceException("Error on translate work item to task info: workItem.Id is required.");

        return new TaskInfo(
            IntegrationTypeEnum.TFS,
            integrationBase.Id.Value,
            integrationBase.Name,
            workItemId,
            $"{workItem.Fields["System.Title"]} (#{workItem.Id})");
    }
}
