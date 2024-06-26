﻿using Pomodorium.Models;
using Pomodorium.Models.Tasks.Integrations;

namespace TeamFoundationServer.Integrations;

public class WorkItemAdapter
{
    private readonly TfsFacade _tfsFacade;

    public WorkItemAdapter(TfsFacade tfsFacade)
    {
        _tfsFacade = tfsFacade;
    }

    public async Task<IEnumerable<TaskInfo>> GetTaskInfoList(TfsIntegration tfsIntegration)
    {
        var workItems = await _tfsFacade.GetWorkItems(tfsIntegration).ConfigureAwait(false);

        var taskInfoList = workItems.Select(x => x.ToTaskInfo(tfsIntegration));

        return taskInfoList;
    }
}
