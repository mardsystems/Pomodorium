using FlowtimeTechnique.Models;
using Microsoft.Extensions.Logging;
using System.ApplicationModel;

namespace FlowtimeTechnique.Features.FlowTimer;

public class FlowtimeArchivingHandler : IRequestHandler<FlowtimeArchivingRequest, FlowtimeArchivingResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly Repository _repository;

    private readonly ILogger<FlowtimeArchivingHandler> _logger;

    public FlowtimeArchivingHandler(IUnitOfWork unitOfWork, Repository repository, ILogger<FlowtimeArchivingHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _logger = logger;
    }

    public async Task<FlowtimeArchivingResponse> Handle(FlowtimeArchivingRequest request, CancellationToken cancellationToken)
    {
        var transaction = _unitOfWork.BeginTransactionFor(request, _logger);

        try
        {
            var flowtime = await _repository.GetAggregateById<Flowtime>(request.FlowtimeId) ?? throw new EntityNotFoundException();

            flowtime.Archive();

            await _repository.Save(flowtime, request.FlowtimeVersion ?? -1);

            transaction.Commit();

            var response = new FlowtimeArchivingResponse(request.GetCorrelationId())
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
