using Pomodorium.Drivers;
using Pomodorium.Features.TaskManager;
using Pomodorium.Support;

namespace Pomodorium.StepDefinitions
{
    [Binding]
    public class TaskManagerStepDefinitions
    {
        private readonly TaskManagerApiDriver _taskManagerApiDriver;

        private Guid _taskId;

        private string _taskDescription;

        private long _taskVersion;

        public TaskManagerStepDefinitions(TaskManagerApiDriver taskManagerApiDriver)
        {
            _taskManagerApiDriver = taskManagerApiDriver;
        }

        [Given(@"that there is any customer")]
        public void GivenThatThereIsAnyCustomer()
        {
            var request = TaskManagerStubs.CreateTask();

            var response = _taskManagerApiDriver.CreateTaskAction.Perform(request, true);

            _taskId = response.TaskId;

            _taskVersion = response.TaskVersion;
        }

        [When(@"Programmer registers a task '([^']*)'")]
        public void WhenProgrammerRegistersATask(string description)
        {
            _taskDescription = description;

            var request = new CreateTaskRequest
            {
                Description = _taskDescription
            };

            var response = _taskManagerApiDriver.CreateTaskAction.Perform(request);

            _taskId = response.TaskId;

            _taskVersion = response.TaskVersion;
        }

        [When(@"Programmer change a task to '([^']*)'")]
        public void WhenProgrammerChangeATaskTo(string description)
        {
            _taskDescription = description;

            var request = new ChangeTaskDescriptionRequest
            {
                TaskId = _taskId,
                Description = _taskDescription,
                TaskVersion = _taskVersion
            };

            var response = _taskManagerApiDriver.ChangeTaskDescriptionAction.Perform(request);

            _taskVersion = response.TaskVersion;
        }

        [Then(@"the task should be registered as expected")]
        public void ThenTheTaskShouldBeRegisteredAsExpected()
        {
            var task = _taskManagerApiDriver.GetTaskAction.Perform(new GetTaskRequest(_taskId)).TaskDetails;

            task.Description.Should().Be(_taskDescription);
        }
    }
}
