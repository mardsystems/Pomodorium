using Pomodorium.Models.Flows;
using Microsoft.Extensions.Logging;
using System.ApplicationModel;

namespace Pomodorium.Features.FlowTimer;

public class FlowtimeCreationHandler : IRequestHandler<FlowtimeCreationRequest, FlowtimeCreationResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly Repository _repository;

    private readonly ILogger<FlowtimeCreationHandler> _logger;

    public FlowtimeCreationHandler(IUnitOfWork unitOfWork, Repository repository, ILogger<FlowtimeCreationHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _logger = logger;
    }

    public async Task<FlowtimeCreationResponse> Handle(FlowtimeCreationRequest request, CancellationToken cancellationToken)
    {
        var transaction = _unitOfWork.BeginTransactionFor(request, _logger);

        try
        {
            var taskId = Guid.NewGuid();

            var task = new Pomodorium.Models.Tasks.Task(taskId, request.TaskDescription, transaction);

            await _repository.Save(task);

            var flowtimeId = Guid.NewGuid();

            var flowtime = new Flowtime(flowtimeId, task, transaction);

            await _repository.Save(flowtime);

            transaction.Commit();

            var response = new FlowtimeCreationResponse(transaction.CorrelationId)
            {
                FlowtimeId = flowtime.Id,
                FlowtimeVersion = flowtime.Version
            };

            return response;
        }
        catch (Exception)
        {
            transaction.Rollback();

            throw;
        }
    }
}
