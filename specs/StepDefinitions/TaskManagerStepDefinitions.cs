using Pomodorium.Drivers;
using Pomodorium.Support;
using TechTalk.SpecFlow.Assist;

namespace Pomodorium.StepDefinitions
{
    [Binding]
    public class TaskManagerStepDefinitions
    {
        private readonly TaskManagerApiDriver _taskManagerApiDriver;

        private Table _specification;

        public TaskManagerStepDefinitions(TaskManagerApiDriver taskManagerApiDriver)
        {
            _taskManagerApiDriver = taskManagerApiDriver;
        }

        [Given(@"that there is any customer")]
        public async void GivenThatThereIsAnyCustomer()
        {
            var request = TaskManagerStubs.CreateTask();

            var response = _taskManagerApiDriver.CreateTask(request);

            var task = _taskManagerApiDriver.GetTask(response.TaskId).TaskDetails;
        }

        [When(@"Marcelo registers a task as")]
        public void WhenMarceloRegistersATaskAs(Table table)
        {
            _specification = table;

            var request = table.CreateInstance(TaskManagerStubs.CreateTask);

            var response = _taskManagerApiDriver.CreateTaskAction.Perform(request);
        }

        [Then(@"the task should be registered as expected")]
        public void ThenTheTaskShouldBeRegisteredAsExpected()
        {
            var tasks = _taskManagerApiDriver.GetTasks().TaskQueryItems;

            _specification.CompareToSet(tasks);
        }
    }
}
