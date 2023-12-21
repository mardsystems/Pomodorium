using MediatR;
using Pomodorium.Features.TaskManager;
using Pomodorium.Integrations.Trello;
using Pomodorium.Models.TaskManagement.Integrations;
using Pomodorium.Repositories;
using System.DomainModel;

namespace Pomodorium.Features.TaskSynchronizer;

public class TaskSyncFromTrelloHandler : IRequestHandler<TaskSyncFromTrelloRequest, TaskSyncFromTrelloResponse>
{
    private readonly IMediator _mediator;

    private readonly ITrelloIntegrationRepository _trelloIntegrationRepository;

    private readonly CardAdapter _listsAdapter;

    private readonly Repository _repository;

    public TaskSyncFromTrelloHandler(
        IMediator mediator,
        ITrelloIntegrationRepository trelloIntegrationRepository,
        Repository repository,
        CardAdapter listsAdapter)
    {
        _mediator = mediator;

        _trelloIntegrationRepository = trelloIntegrationRepository;

        _repository = repository;

        _listsAdapter = listsAdapter;
    }

    public async Task<TaskSyncFromTrelloResponse> Handle(TaskSyncFromTrelloRequest request, CancellationToken cancellationToken)
    {
        var trelloIntegrationList = await _trelloIntegrationRepository.GetTrelloIntegrationList();

        foreach (var trelloIntegration in trelloIntegrationList)
        {
            var taskInfoList = await _listsAdapter.GetTaskInfoList(trelloIntegration).ConfigureAwait(false);

            foreach (var taskInfo in taskInfoList)
            {
                var getTasksRequest = new TaskQueryRequest
                {
                    ExternalReference = taskInfo.Reference
                };

                var getTasksResponse = await _mediator.Send<TaskQueryResponse>(getTasksRequest);

                var taskQueryItem = getTasksResponse.TaskQueryItems.FirstOrDefault();

                Models.TaskManagement.Tasks.Task task;

                if (taskQueryItem == default)
                {
                    task = new Models.TaskManagement.Tasks.Task(taskInfo.Name);
                }
                else
                {
                    var taskExisting = await _repository.GetAggregateById<Models.TaskManagement.Tasks.Task>(taskQueryItem.Id);

                    if (taskExisting == null)
                    {
                        task = new Models.TaskManagement.Tasks.Task(taskInfo.Name);
                    }
                    else
                    {
                        task = taskExisting;

                        if (task.Description != taskInfo.Name)
                        {
                            task.ChangeDescription(taskInfo.Name);
                        }
                    }
                }

                await _repository.Save(task, -1);

                var taskIntegration = new TaskIntegration(task, taskInfo);

                await _repository.Save(taskIntegration, -1);
            }
        }

        var response = new TaskSyncFromTrelloResponse(request.GetCorrelationId()) { };

        return response;
    }
}
