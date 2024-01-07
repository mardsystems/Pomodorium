using FlowtimeTechnique.Models;
using Microsoft.Extensions.Logging;
using System.ApplicationModel;

namespace FlowtimeTechnique.Features.FlowTimer;

public class FlowtimeStopHandler : IRequestHandler<FlowtimeStopRequest, FlowtimeStopResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly Repository _repository;

    private readonly ILogger<FlowtimeStopHandler> _logger;
    
    public FlowtimeStopHandler(IUnitOfWork unitOfWork, Repository repository, ILogger<FlowtimeStopHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _logger = logger;
    }

    public async Task<FlowtimeStopResponse> Handle(FlowtimeStopRequest request, CancellationToken cancellationToken)
    {
        var transaction = _unitOfWork.BeginTransactionFor(request, _logger);

        try
        {
            var flowtime = await _repository.GetAggregateById<Flowtime>(request.FlowtimeId) ?? throw new EntityNotFoundException();

            var now = DateTime.Now;

            flowtime.Stop(now);

            await _repository.Save(flowtime, request.FlowtimeVersion ?? -1);

            transaction.Commit();

            var response = new FlowtimeStopResponse(request.GetCorrelationId())
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
