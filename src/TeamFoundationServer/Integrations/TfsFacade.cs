﻿using Microsoft.Extensions.Options;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Pomodorium.Models;

namespace TeamFoundationServer.Integrations;

public class TfsFacade
{
    private readonly TfsIntegrationOptions _tfsIntegrationOptions;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TfsFacade" /> class.
    /// </summary>
    public TfsFacade(IOptions<TfsIntegrationOptions> optionsInterface)
    {
        _tfsIntegrationOptions = optionsInterface.Value;
    }

    /// <summary>
    ///     Execute a WIQL (Work Item Query Language) query to return a list of open bugs.
    /// </summary>
    /// <param name="project">The name of your project within your organization.</param>
    /// <returns>A list of <see cref="WorkItem"/> objects representing all the open bugs.</returns>
    public virtual async Task<IEnumerable<WorkItem>> GetWorkItems(TfsIntegration tfsIntegration)
    {
        var uri = new Uri($"{_tfsIntegrationOptions.BaseAddress}/{tfsIntegration.OrganizationName}");

        var credentials = new VssBasicCredential(string.Empty, tfsIntegration.PersonalAccessToken);

        // create a wiql object and build our query
        var wiql = new Wiql()
        {
            // NOTE: Even if other columns are specified, only the ID & URL are available in the WorkItemReference
            Query = "Select [Id] " +
                    "From WorkItems " +
                    "Where [Work Item Type] = 'Task' " +
                    "And [System.TeamProject] = '" + tfsIntegration.ProjectName + "' " +
                    //"And [System.State] <> 'Closed' " +
                    "Order By [State] Asc, [Changed Date] Desc",
        };

        // create instance of work item tracking http client
        using var httpClient = new WorkItemTrackingHttpClient(uri, credentials);

        // execute the query to get the list of work items in the results
        var result = await httpClient.QueryByWiqlAsync(wiql).ConfigureAwait(false);

        var ids = result.WorkItems.Select(item => item.Id).ToArray();

        // some error handling
        if (ids.Length == 0)
        {
            return Array.Empty<WorkItem>();
        }

        // build a list of the fields we want to see
        var fields = new[] { "System.Id", "System.Title", "System.State" };

        // get work items for the ids found in query
        var workItems = await httpClient.GetWorkItemsAsync(ids, fields, result.AsOf).ConfigureAwait(false);

        return workItems;
    }
}
