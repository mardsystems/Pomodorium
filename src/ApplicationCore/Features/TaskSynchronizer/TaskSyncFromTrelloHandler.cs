using Microsoft.Extensions.Logging;
using Pomodorium.Features.TaskManager;
using Pomodorium.Repositories;
using System.ApplicationModel;
using Pomodorium.Models.Tasks.Integrations;

namespace Pomodorium.Features.TaskSynchronizer;

public class TaskSyncFromTrelloHandler : IRequestHandler<TaskSyncFromTrelloRequest, TaskSyncFromTrelloResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly IMediator _mediator;

    private readonly ITrelloIntegrationRepository _trelloIntegrationRepository;

    private readonly ITrelloIntegrationService _trelloIntegrationService;

    private readonly Repository _repository;

    private readonly ILogger<TaskSyncFromTrelloHandler> _logger;

    public TaskSyncFromTrelloHandler(
        IUnitOfWork unitOfWork,
        IMediator mediator,
        ITrelloIntegrationRepository trelloIntegrationRepository,
        Repository repository,
        ITrelloIntegrationService trelloIntegrationService,
        ILogger<TaskSyncFromTrelloHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _trelloIntegrationRepository = trelloIntegrationRepository;
        _repository = repository;
        _trelloIntegrationService = trelloIntegrationService;
        _logger = logger;
    }

    public async Task<TaskSyncFromTrelloResponse> Handle(TaskSyncFromTrelloRequest request, CancellationToken cancellationToken)
    {
        var transaction = _unitOfWork.BeginTransactionFor(request, _logger);

        try
        {
            var trelloIntegrationList = await _trelloIntegrationRepository.GetTrelloIntegrationList(cancellationToken: cancellationToken);

            foreach (var trelloIntegration in trelloIntegrationList)
            {
                var taskInfoList = await _trelloIntegrationService.GetTaskInfoList(trelloIntegration).ConfigureAwait(false);

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

            var response = new TaskSyncFromTrelloResponse(request.GetCorrelationId()) { };

            return response;
        }
        catch (Exception ex)
        {
            transaction.Rollback(ex);

            throw;
        }
    }
}
