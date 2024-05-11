using Pomodorium.Models.Flows;
using Microsoft.Extensions.Logging;
using System.ApplicationModel;

namespace Pomodorium.Features.FlowTimer;

public class FlowtimeStartHandler : IRequestHandler<FlowtimeStartRequest, FlowtimeStartResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly Repository _repository;

    private readonly ILogger<FlowtimeStartHandler> _logger;

    public FlowtimeStartHandler(IUnitOfWork unitOfWork, Repository repository, ILogger<FlowtimeStartHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _logger = logger;
    }

    public async Task<FlowtimeStartResponse> Handle(FlowtimeStartRequest request, CancellationToken cancellationToken)
    {
        var transaction = _unitOfWork.BeginTransactionFor(request, _logger);

        try
        {
            var flowtime = await _repository.GetAggregateById<Flowtime>(request.FlowtimeId) ?? throw new EntityNotFoundException();

            var now = DateTime.Now;

            flowtime.Start(now);

            await _repository.Save(flowtime, request.FlowtimeVersion ?? -1);

            transaction.Commit();

            var response = new FlowtimeStartResponse(request.GetCorrelationId())
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
