using MediatR;
using Pomodorium.Features.TaskManager;
using Pomodorium.Integrations.TFS;
using Pomodorium.Models.TaskManagement.Integrations;
using Pomodorium.Repositories;
using System.DomainModel;

namespace Pomodorium.Features.TaskSynchronizer;

public class TaskSyncFromTfsHandler : IRequestHandler<TaskSyncFromTfsRequest, TaskSyncFromTfsResponse>
{
    private readonly IMediator _mediator;

    private readonly ITfsIntegrationRepository _tfsIntegrationRepository;

    private readonly WorkItemAdapter _workItemAdapter;

    private readonly Repository _repository;

    public TaskSyncFromTfsHandler(
        IMediator mediator,
        ITfsIntegrationRepository tfsIntegrationRepository,
        Repository repository,
        WorkItemAdapter workItemAdapter)
    {
        _mediator = mediator;

        _tfsIntegrationRepository = tfsIntegrationRepository;

        _repository = repository;

        _workItemAdapter = workItemAdapter;
    }

    public async Task<TaskSyncFromTfsResponse> Handle(TaskSyncFromTfsRequest request, CancellationToken cancellationToken)
    {
        var tfsIntegrationList = await _tfsIntegrationRepository.GetTfsIntegrationList(cancellationToken: cancellationToken);

        foreach (var tfsIntegration in tfsIntegrationList)
        {
            var taskInfoList = await _workItemAdapter.GetTaskInfoList(tfsIntegration).ConfigureAwait(false);

            foreach (var taskInfo in taskInfoList)
            {
                var getTasksRequest = new TaskQueryRequest
                {
                    ExternalReference = taskInfo.Reference
                };

                var getTasksResponse = await _mediator.Send<TaskQueryResponse>(getTasksRequest, cancellationToken);

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

        var response = new TaskSyncFromTfsResponse(request.GetCorrelationId());

        return response;
    }
}
