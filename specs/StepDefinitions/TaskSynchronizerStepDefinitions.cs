using Pomodorium.Drivers;
using Pomodorium.Features.TaskManager;
using Pomodorium.Features.TaskSynchronizer;
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

    private Table _specification;

    public TaskSynchronizerStepDefinitions(
        ScenarioContext scenario,
        TaskSynchronizerApiDriver taskSynchronizerApiDriver,
        TaskManagerApiDriver taskManagerApiDriver)
    {
        _scenario = scenario;
        _taskSynchronizerApiDriver = taskSynchronizerApiDriver;
        _taskManagerApiDriver = taskManagerApiDriver;
    }

    [Given(@"User starts a task synch with TFS")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    public void GivenUserStartsATaskSynchWithTFS()
    {

    }

    [When(@"User request task synch")]
    public void WhenUserRequestTaskSynch()
    {
        var request = new TaskSyncFromTfsRequest
        {

        };

        var _ = _taskSynchronizerApiDriver.SynchTasksAction.Perform(request);
    }

    [Then(@"System should get workitems from TFS\. Examples:")]
    public void ThenSystemShouldGetWorkitemsFromTFS_Examples(Table table)
    {
        _specification = table;

        var _ = _specification.CreateSet<WorkItemData>();
    }

    [Then(@"System should create tasks")]
    public void ThenSystemShouldCreateTasks()
    {
        var tasks = _taskManagerApiDriver.QueryTasksAction.Perform(new TaskQueryRequest()).TaskQueryItems;

        tasks.Should().HaveCount(3);

        _scenario["Tasks"] = tasks;
    }

    [Then(@"System should translates workitem system title to task description")]
    public void ThenSystemShouldTranslatesWorkitemSystemTitleToTaskDescription()
    {
        var tasks = (IEnumerable<TaskQueryItem>)_scenario["Tasks"];

        var workitems = _specification.CreateSet<WorkItemData>();

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

public class WorkItemData
{
    public string SystemTitle { get; set; }
}