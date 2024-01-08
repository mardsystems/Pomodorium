using MediatR;
using Microsoft.Extensions.Logging;
using TaskManagement.Features.TaskManager;
using Pomodorium.Integrations.TFS;
using Pomodorium.Repositories;
using System.ApplicationModel;
using System.DomainModel;
using TaskManagement.Models.Integrations;

namespace TaskManagement.Features.TaskSynchronizer;

public class TaskSyncFromTfsHandler : IRequestHandler<TaskSyncFromTfsRequest, TaskSyncFromTfsResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly IMediator _mediator;

    private readonly ITfsIntegrationRepository _tfsIntegrationRepository;

    private readonly WorkItemAdapter _workItemAdapter;

    private readonly Repository _repository;

    private readonly ILogger<TaskSyncFromTfsHandler> _logger;

    public TaskSyncFromTfsHandler(
        IUnitOfWork unitOfWork,
        IMediator mediator,
        ITfsIntegrationRepository tfsIntegrationRepository,
        Repository repository,
        WorkItemAdapter workItemAdapter,
        ILogger<TaskSyncFromTfsHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _tfsIntegrationRepository = tfsIntegrationRepository;
        _repository = repository;
        _workItemAdapter = workItemAdapter;
        _logger = logger;
    }

    public async Task<TaskSyncFromTfsResponse> Handle(TaskSyncFromTfsRequest request, CancellationToken cancellationToken)
    {
        var transaction = _unitOfWork.BeginTransactionFor(request, _logger);

        try
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

                    TaskManagement.Models.Tasks.Task task;

                    var taskId = Guid.NewGuid();

                    if (taskQueryItem == default)
                    {
                        task = new TaskManagement.Models.Tasks.Task(taskId, taskInfo.Name, transaction);
                    }
                    else
                    {
                        var taskExisting = await _repository.GetAggregateById<TaskManagement.Models.Tasks.Task>(taskQueryItem.Id);

                        if (taskExisting == null)
                        {
                            task = new TaskManagement.Models.Tasks.Task(taskId, taskInfo.Name, transaction);
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

                    await _repository.Save(task);

                    var taskIntegration = new TaskIntegration(taskId, task, taskInfo, transaction);

                    await _repository.Save(taskIntegration);
                }
            }

            transaction.Commit();

            var response = new TaskSyncFromTfsResponse(request.GetCorrelationId());

            return response;
        }
        catch (Exception ex)
        {
            transaction.Rollback(ex);

            throw;
        }
    }
}
