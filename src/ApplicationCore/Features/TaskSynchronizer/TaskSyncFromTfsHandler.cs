using Microsoft.Extensions.Logging;
using Pomodorium.Features.TaskManager;
using Pomodorium.Repositories;
using System.ApplicationModel;
using Pomodorium.Models.Tasks.Integrations;

namespace Pomodorium.Features.TaskSynchronizer;

public class TaskSyncFromTfsHandler : IRequestHandler<TaskSyncFromTfsRequest, TaskSyncFromTfsResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly IMediator _mediator;

    private readonly ITfsIntegrationRepository _tfsIntegrationRepository;

    private readonly ITfsIntegrationService _tfsIntegrationService;

    private readonly Repository _repository;

    private readonly ILogger<TaskSyncFromTfsHandler> _logger;

    public TaskSyncFromTfsHandler(
        IUnitOfWork unitOfWork,
        IMediator mediator,
        ITfsIntegrationRepository tfsIntegrationRepository,
        Repository repository,
        ITfsIntegrationService tfsIntegrationService,
        ILogger<TaskSyncFromTfsHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _tfsIntegrationRepository = tfsIntegrationRepository;
        _repository = repository;
        _tfsIntegrationService = tfsIntegrationService;
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
                var taskInfoList = await _tfsIntegrationService.GetTaskInfoList(tfsIntegration).ConfigureAwait(false);

                foreach (var taskInfo in taskInfoList)
                {
                    var getTasksRequest = new TaskQueryRequest
                    {
                        ExternalReference = taskInfo.Reference
                    };

                    var getTasksResponse = await _mediator.Send<TaskQueryResponse>(getTasksRequest, cancellationToken);

                    var taskQueryItem = getTasksResponse.TaskQueryItems.FirstOrDefault();

                    Pomodorium.Models.Tasks.Task task;

                    var taskId = Guid.NewGuid();

                    if (taskQueryItem == default)
                    {
                        task = new Pomodorium.Models.Tasks.Task(taskId, taskInfo.Name, transaction);
                    }
                    else
                    {
                        var taskExisting = await _repository.GetAggregateById<Pomodorium.Models.Tasks.Task>(taskQueryItem.Id);

                        if (taskExisting == null)
                        {
                            task = new Pomodorium.Models.Tasks.Task(taskId, taskInfo.Name, transaction);
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
