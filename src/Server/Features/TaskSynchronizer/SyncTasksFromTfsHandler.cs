using MediatR;
using Pomodorium.Features.TaskManager;
using Pomodorium.TeamFoundationServer;
using System.DomainModel;

namespace Pomodorium.Features.TaskSynchronizer;

public class SyncTasksFromTfsHandler : IRequestHandler<SyncTasksFromTfsRequest, SyncTasksFromTfsResponse>
{
    private readonly IMediator _mediator;

    private readonly WorkItemAdapter _workItemAdapter;

    private readonly Repository _repository;

    public SyncTasksFromTfsHandler(
        IMediator mediator,
        Repository repository,
        WorkItemAdapter workItemAdapter)
    {
        _mediator = mediator;

        _repository = repository;

        _workItemAdapter = workItemAdapter;
    }

    public async Task<SyncTasksFromTfsResponse> Handle(SyncTasksFromTfsRequest request, CancellationToken cancellationToken)
    {
        var getTfsIntegrationListRequest = new GetTfsIntegrationListRequest
        {

        };

        var getTfsIntegrationListResponse = await _mediator.Send<GetTfsIntegrationListResponse>(getTfsIntegrationListRequest);

        foreach (var tfsIntegration in getTfsIntegrationListResponse.TfsIntegrationList)
        {
            var taskInfoList = await _workItemAdapter.GetTaskInfoListBy(tfsIntegration).ConfigureAwait(false);

            foreach (var taskInfo in taskInfoList)
            {
                var getTasksRequest = new GetTasksRequest
                {
                    ExternalSourceId = taskInfo.Reference
                };

                var getTasksResponse = await _mediator.Send<GetTasksResponse>(getTasksRequest);

                var taskQueryItem = getTasksResponse.TaskQueryItems.FirstOrDefault();

                TaskManagement.Model.Tasks.Task task;

                if (taskQueryItem == default)
                {
                    task = new TaskManagement.Model.Tasks.Task(taskInfo.Name, taskInfo.Reference);
                }
                else
                {
                    var taskExisting = await _repository.GetAggregateById<TaskManagement.Model.Tasks.Task>(taskQueryItem.Id);

                    if (taskExisting == null)
                    {
                        task = new TaskManagement.Model.Tasks.Task(taskInfo.Name, taskInfo.Reference);
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
            }
        }

        var response = new SyncTasksFromTfsResponse(request.GetCorrelationId()) { };

        return response;
    }
}
