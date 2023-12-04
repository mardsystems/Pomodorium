using Pomodorium.Drivers;
using Pomodorium.Features.TaskManager;
using Pomodorium.Support;
using TechTalk.SpecFlow.Assist;

namespace Pomodorium.StepDefinitions
{
    [Binding]
    public class TaskManagerStepDefinitions
    {
        private readonly TaskManagerApiDriver _taskManagerApiDriver;

        private Table _specification;

        private Guid _taskId;

        public TaskManagerStepDefinitions(TaskManagerApiDriver taskManagerApiDriver)
        {
            _taskManagerApiDriver = taskManagerApiDriver;
        }

        [Given(@"that there is any customer")]
        public async void GivenThatThereIsAnyCustomer()
        {
            var request = TaskManagerStubs.CreateTask();

            var response = _taskManagerApiDriver.CreateTask(request);

            _taskId = _taskManagerApiDriver.GetTask(response.TaskId).TaskDetails.Id;
        }

        [When(@"Programmer registers a task as")]
        public void WhenProgrammerRegistersATaskAs(Table table)
        {
            _specification = table;

            var request = table.CreateInstance(TaskManagerStubs.CreateTask);

            var response = _taskManagerApiDriver.CreateTaskAction.Perform(request);
        }

        [When(@"Programmer change a task as")]
        public void WhenProgrammerChangeATaskAs(Table table)
        {
            _specification = table;

            var request = table.CreateInstance(TaskManagerStubs.ChangeTaskDescription);

            request.TaskId = _taskId;
            
            var response = _taskManagerApiDriver.ChangeTaskDescriptionAction.Perform(request);
        }

        [Then(@"the task should be registered as expected")]
        public void ThenTheTaskShouldBeRegisteredAsExpected()
        {
            var tasks = _taskManagerApiDriver.GetTasks(new GetTasksRequest()).TaskQueryItems;

            _specification.CompareToSet(tasks);
        }
    }
}
