using Pomodorium.Drivers;
using Pomodorium.Features.TaskManager;
using Pomodorium.Features.TaskSynchronizer;
using Pomodorium.Models;
using Pomodorium.Support;
using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Pomodorium.StepDefinitions;

[Binding]
public class TaskSynchronizerStepDefinitions
{
    private readonly ScenarioContext _scenario;

    private readonly TaskSynchronizerApiDriver _taskSynchronizerApiDriver;

    private readonly TaskManagerApiDriver _taskManagerApiDriver;

    private readonly TaskSynchronizerContext _taskSynchronizer;

    public TaskSynchronizerStepDefinitions(
        ScenarioContext scenario,
        TaskSynchronizerApiDriver taskSynchronizerApiDriver,
        TaskManagerApiDriver taskManagerApiDriver,
        TaskSynchronizerContext taskSynchronizer)
    {
        _scenario = scenario;
        _taskSynchronizerApiDriver = taskSynchronizerApiDriver;
        _taskManagerApiDriver = taskManagerApiDriver;
        _taskSynchronizer = taskSynchronizer;
    }

    [Given(@"exists a TFS integration settings as")]
    public void GivenExistsATFSIntegrationSettingsAs(Table table)
    {
        var tfsIntegrationList = table.CreateSet<TfsIntegration>();

        foreach (var tfsIntegration in tfsIntegrationList)
        {
            var _ = _taskSynchronizerApiDriver.PostTfsIntegrationAction.Perform(tfsIntegration);
        }        
    }

    [Given(@"User starts a task synch with TFS")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    public void GivenUserStartsATaskSynchWithTFS()
    {

    }

    [Given(@"exists a workitems in TFS as")]
    public void GivenExistsAWorkitemsInTFSAs(Table table)
    {
        _taskSynchronizer.Specification = table;

        var _ = _taskSynchronizer.Specification.CreateSet<WorkItemData>();
    }

    [When(@"User request task synch with TFS")]
    public void WhenUserRequestTaskSynchWithTFS()
    {
        var request = new TaskSyncFromTfsRequest
        {

        };

        var _ = _taskSynchronizerApiDriver.SynchTasksAction.Perform(request);
    }

    [Then(@"System should get workitems from TFS")]
    public void ThenSystemShouldGetWorkitemsFromTFS()
    {

    }

    [Then(@"System should create tasks as")]
    public void ThenSystemShouldCreateTasksAs(Table table)
    {
        var tasks = _taskManagerApiDriver.QueryTasksAction.Perform(new TaskQueryRequest()).TaskQueryItems;

        tasks.Should().HaveCount(3);

        _scenario["Tasks"] = tasks;

        table.CompareToSet(tasks);
    }

    [Then(@"System should translates workitem system title to task description")]
    public void ThenSystemShouldTranslatesWorkitemSystemTitleToTaskDescription()
    {
        var tasks = (IEnumerable<TaskQueryItem>)_scenario["Tasks"];

        var workitems = _taskSynchronizer.Specification.CreateSet<WorkItemData>();

        workitems.Should().BeEquivalentTo(tasks);

        //foreach (var row in _specification.Rows)
        //{

        //}

        //foreach (var task in tasks)
        //{
        //    task.Description.Should().Be(description);
        //}
    }

    [Then(@"task description should be equal to workitem system title")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    public void ThenTaskDescriptionShouldBeEqualToWorkitemSystemTitle()
    {
        
    }
}
