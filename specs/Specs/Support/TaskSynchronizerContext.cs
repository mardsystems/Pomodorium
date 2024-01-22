using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using TechTalk.SpecFlow.Assist;

namespace Pomodorium.Support;

public class TaskSynchronizerContext
{
    public Table Specification { get; set; }

    public IEnumerable<WorkItem> GetWorkItems()
    {
        var workItemsData = Specification.CreateSet<WorkItemData>().ToArray();

        var workItems = new List<WorkItem>();

        for (int i = 0; i < workItemsData.Length; i++)
        {
            var data = workItemsData[i];

            workItems.Add(new WorkItem()
            {
                Id = i + 1,
                Fields =
                {
                    { "System.Title", data.SystemTitle }
                }
            });
        }

        return workItems;
    }
}

public class WorkItemData
{
    public string SystemTitle { get; set; }
}

public class TaskData
{
    public string Description { get; set; }
}