using Microsoft.Extensions.Logging;
using System.ApplicationModel;

namespace Pomodorium.Features.TaskManager;

public class TaskRegistrationHandler : IRequestHandler<TaskRegistrationRequest, TaskRegistrationResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly Repository _repository;

    private readonly ILogger<TaskRegistrationHandler> _logger;

    public TaskRegistrationHandler(IUnitOfWork unitOfWork, Repository repository, ILogger<TaskRegistrationHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _logger = logger;
    }

    public async Task<TaskRegistrationResponse> Handle(TaskRegistrationRequest request, CancellationToken cancellationToken)
    {
        var transaction = _unitOfWork.BeginTransactionFor(request, _logger);

        try
        {
            var task = new Models.TaskManagement.Tasks.Task(transaction.CorrelationId, request.Description, transaction);

            await _repository.Save(task);

            transaction.Commit();

            var response = new TaskRegistrationResponse(transaction.CorrelationId)
            {
                TaskId = task.Id,
                TaskVersion = task.Version,
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
