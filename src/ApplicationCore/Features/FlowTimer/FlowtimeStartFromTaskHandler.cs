using Microsoft.Extensions.Logging;
using Pomodorium.Models.FlowtimeTechnique;
using System.ApplicationModel;

namespace Pomodorium.Features.FlowTimer;

public class FlowtimeStartFromTaskHandler : IRequestHandler<FlowtimeStartFromTaskRequest, FlowtimeStartFromTaskResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly Repository _repository;

    private readonly ILogger<FlowtimeStartFromTaskHandler> _logger;
    
    public FlowtimeStartFromTaskHandler(IUnitOfWork unitOfWork, Repository repository, ILogger<FlowtimeStartFromTaskHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _logger = logger;
    }

    public async Task<FlowtimeStartFromTaskResponse> Handle(FlowtimeStartFromTaskRequest request, CancellationToken cancellationToken)
    {
        var transaction = _unitOfWork.BeginTransactionFor(request, _logger);

        try
        {
            var task = await _repository.GetAggregateById<Models.TaskManagement.Tasks.Task>(request.TaskId);

            var flowtimeId = Guid.NewGuid();

            var flowtime = new Flowtime(flowtimeId, task, transaction);

            var now = DateTime.Now;

            flowtime.Start(now);

            await _repository.Save(flowtime);

            transaction.Commit();

            var response = new FlowtimeStartFromTaskResponse(request.GetCorrelationId())
            {
                FlowtimeVersion = flowtime.Version
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
