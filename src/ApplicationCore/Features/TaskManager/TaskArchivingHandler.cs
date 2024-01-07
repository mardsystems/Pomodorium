using Microsoft.Extensions.Logging;
using System.ApplicationModel;

namespace Pomodorium.Features.TaskManager;

public class TaskArchivingHandler : IRequestHandler<TaskArchivingRequest, TaskArchivingResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly Repository _repository;

    private readonly ILogger<TaskArchivingHandler> _logger;
    
    public TaskArchivingHandler(IUnitOfWork unitOfWork, Repository repository, ILogger<TaskArchivingHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _logger = logger;
    }

    public async Task<TaskArchivingResponse> Handle(TaskArchivingRequest request, CancellationToken cancellationToken)
    {
        var transaction = _unitOfWork.BeginTransactionFor(request, _logger);

        try
        {
            var task = await _repository.GetAggregateById<TaskManagement.Models.Tasks.Task>(request.TaskId) ?? throw new EntityNotFoundException();

            task.Archive();

            await _repository.Save(task, request.TaskVersion ?? -1);

            transaction.Commit();

            var response = new TaskArchivingResponse(request.GetCorrelationId())
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
