using Pomodorium.Drivers;
using TaskManagement.Features.TaskManager;
using Pomodorium.Support;

namespace Pomodorium.StepDefinitions;

[Binding]
public class TaskManagerStepDefinitions
{
    private readonly ScenarioContext _scenario;

    private readonly TaskManagerApiDriver _taskManagerApiDriver;

    public TaskManagerStepDefinitions(ScenarioContext scenario, TaskManagerApiDriver taskManagerApiDriver)
    {
        _scenario = scenario;

        _taskManagerApiDriver = taskManagerApiDriver;
    }

    [Given(@"User starts a task registration")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    public void GivenUserStartsATaskRegistration()
    {
        
    }

    [Given(@"User inputs task description as '([^']*)'")]
    public void GivenUserInputsTaskAsDescription(string description)
    {
        _scenario["Description"] = description;
    }

    [When(@"User register task")]
    public void WhenUserRegisterTask()
    {
        var description = (string)_scenario["Description"];

        var request = new TaskRegistrationRequest
        {
            Description = description
        };

        var response = _taskManagerApiDriver.CreateTaskAction.Perform(request);

        _scenario["TaskId"] = response.TaskId;

        _scenario["TaskVersion"] = response.TaskVersion;
    }

    [Then(@"System should create a task as expected")]
    public void ThenSystemShouldCreateATaskAsExpected()
    {
        var taskId = (Guid)_scenario["TaskId"];

        var task = _taskManagerApiDriver.GetTaskAction.Perform(new TaskDetailsRequest(taskId)).TaskDetails;

        var description = (string)_scenario["Description"];

        task.Description.Should().Be(description);
    }

    [Given(@"User starts a change task description")]
    public void GivenUserStartsAChangeTaskDescription()
    {
        var request = TaskManagerStubs.CreateTask();

        var response = _taskManagerApiDriver.CreateTaskAction.Perform(request, true);

        _scenario["TaskId"] = response.TaskId;

        _scenario["TaskVersion"] = response.TaskVersion;
    }

    [When(@"User change task description")]
    public void WhenUserChangeTaskDescription()
    {
        var taskId = (Guid)_scenario["TaskId"];

        var description = (string)_scenario["Description"];

        var taskVersion = (long)_scenario["TaskVersion"];

        var request = new TaskDescriptionChangeRequest
        {
            TaskId = taskId,
            Description = description,
            TaskVersion = taskVersion
        };

        var response = _taskManagerApiDriver.ChangeTaskDescriptionAction.Perform(request);

        _scenario["TaskVersion"] = response.TaskVersion;
    }

    [Then(@"System should change task description as expected")]
    public void ThenSystemShouldChangeTaskDescriptionAsExpected()
    {
        var taskId = (Guid)_scenario["TaskId"];

        var task = _taskManagerApiDriver.GetTaskAction.Perform(new TaskDetailsRequest(taskId)).TaskDetails;

        var description = (string)_scenario["Description"];

        task.Description.Should().Be(description);
    }
}
