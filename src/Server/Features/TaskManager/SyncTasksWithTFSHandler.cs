using MediatR;
using Pomodorium.TeamFoundationServer;
using System.DomainModel;

namespace Pomodorium.Features.TaskManager;

public class SyncTasksWithTFSHandler : IRequestHandler<SyncTasksWithTFSRequest, SyncTasksWithTFSResponse>
{
    private readonly IMediator _mediator;

    private readonly QueryExecutor _workItemAdapter;

    private readonly Repository _repository;

    public SyncTasksWithTFSHandler(
        IMediator mediator,
        Repository repository,
        QueryExecutor workItemAdapter)
    {
        _mediator = mediator;

        _repository = repository;

        _workItemAdapter = workItemAdapter;
    }

    public async Task<SyncTasksWithTFSResponse> Handle(SyncTasksWithTFSRequest request, CancellationToken cancellationToken)
    {
        var workItems = await _workItemAdapter.QueryTasks(request.ProjectName).ConfigureAwait(false);

        foreach (var workItem in workItems)
        {
            var getTasksRequest = new GetTasksRequest
            {
                ExternalSourceId = workItem.Id.ToString()
            };

            var getTasksResponse = await _mediator.Send<GetTasksResponse>(getTasksRequest);

            var taskQueryItem = getTasksResponse.TaskQueryItems.FirstOrDefault();

            var workItemSystemTitle = $"{workItem.Fields["System.Title"]} (#{workItem.Id})";

            TaskManagement.Model.Tasks.Task task;

            if (taskQueryItem == default)
            {
                task = new TaskManagement.Model.Tasks.Task(workItemSystemTitle, workItem.Id.ToString());
            }
            else
            {
                var taskExisting = await _repository.GetAggregateById<TaskManagement.Model.Tasks.Task>(taskQueryItem.Id);

                if (taskExisting == null)
                {
                    task = new TaskManagement.Model.Tasks.Task(workItemSystemTitle, workItem.Id.ToString());
                }
                else
                {
                    task = taskExisting;

                    if (task.Description != workItemSystemTitle)
                    {
                        task.ChangeDescription(workItemSystemTitle);
                    }
                }
            }

            await _repository.Save(task, -1);
        }

        var response = new SyncTasksWithTFSResponse(request.GetCorrelationId()) { };

        return response;
    }
}
