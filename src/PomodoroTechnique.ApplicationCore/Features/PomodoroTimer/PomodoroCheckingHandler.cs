using Microsoft.Extensions.Logging;
using PomodoroTechnique.Models;
using System.ApplicationModel;

namespace PomodoroTechnique.Features.PomodoroTimer;

public class PomodoroCheckingHandler : IRequestHandler<PomodoroCheckingRequest, PomodoroCheckingResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly Repository _repository;

    private readonly ILogger<PomodoroCheckingHandler> _logger;
    
    public PomodoroCheckingHandler(IUnitOfWork unitOfWork, Repository pomodoroRepository, ILogger<PomodoroCheckingHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _repository = pomodoroRepository;
        _logger = logger;
    }

    public async Task<PomodoroCheckingResponse> Handle(PomodoroCheckingRequest request, CancellationToken cancellationToken)
    {
        var transaction = _unitOfWork.BeginTransactionFor(request, _logger);

        try
        {
            var pomodoro = await _repository.GetAggregateById<Pomodoro>(request.Id) ?? throw new EntityNotFoundException();

            pomodoro.Check();

            await _repository.Save(pomodoro, request.Version);

            transaction.Commit();

            var response = new PomodoroCheckingResponse(request.GetCorrelationId()) { };

            return response;
        }
        catch (Exception ex)
        {
            transaction.Rollback(ex);

            throw;
        }
    }
}
