using Pomodorium.Models.Flows;
using Microsoft.Extensions.Logging;
using System.ApplicationModel;

namespace Pomodorium.Features.FlowTimer;

public class FlowtimeInterruptionHandler : IRequestHandler<FlowtimeInterruptionRequest, FlowtimeInterruptionResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly Repository _repository;

    private readonly ILogger<FlowtimeInterruptionHandler> _logger;

    public FlowtimeInterruptionHandler(IUnitOfWork unitOfWork, Repository repository, ILogger<FlowtimeInterruptionHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _logger = logger;
    }

    public async Task<FlowtimeInterruptionResponse> Handle(FlowtimeInterruptionRequest request, CancellationToken cancellationToken)
    {
        var transaction = _unitOfWork.BeginTransactionFor(request, _logger);

        try
        {
            var flowtime = await _repository.GetAggregateById<Flowtime>(request.FlowtimeId) ?? throw new EntityNotFoundException();

            var now = DateTime.Now;

            flowtime.Interrupt(now);

            await _repository.Save(flowtime, request.FlowtimeVersion ?? -1);

            transaction.Commit();

            var response = new FlowtimeInterruptionResponse(request.GetCorrelationId())
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
