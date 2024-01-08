using Microsoft.Extensions.Logging;
using System.ApplicationModel;

namespace TaskManagement.Features.TaskManager;

public class TaskDescriptionChangeHandler : IRequestHandler<TaskDescriptionChangeRequest, TaskDescriptionChangeResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly Repository _repository;

    private readonly ILogger<TaskDescriptionChangeHandler> _logger;
    
    public TaskDescriptionChangeHandler(IUnitOfWork unitOfWork, Repository repository, ILogger<TaskDescriptionChangeHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _logger = logger;
    }

    public async Task<TaskDescriptionChangeResponse> Handle(TaskDescriptionChangeRequest request, CancellationToken cancellationToken)
    {
        var transaction = _unitOfWork.BeginTransactionFor(request, _logger);

        try
        {
            var task = await _repository.GetAggregateById<TaskManagement.Models.Tasks.Task>(request.TaskId) ?? throw new EntityNotFoundException();

            task.ChangeDescription(request.Description);

            await _repository.Save(task, request.TaskVersion ?? -1);

            transaction.Commit();

            var response = new TaskDescriptionChangeResponse(request.GetCorrelationId())
            {
                TaskVersion = task.Version
            };

            return response;
        }
        catch (Exception ex)
        {
            transaction.Rollback(ex);

            throw;
        }
    }
}
