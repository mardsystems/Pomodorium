using Microsoft.Extensions.Logging;
using Pomodorium.Models.Pomos;
using System.ApplicationModel;

namespace Pomodorium.Features.PomodoroTimer;

public class PomodoroCreationHandler : IRequestHandler<PomodoroCreationRequest, PomodoroCreationResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly Repository _repository;

    private readonly ILogger<PomodoroCreationHandler> _logger;
    
    public PomodoroCreationHandler(IUnitOfWork unitOfWork, Repository pomodoroRepository, ILogger<PomodoroCreationHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _repository = pomodoroRepository;
        _logger = logger;
    }

    public async Task<PomodoroCreationResponse> Handle(PomodoroCreationRequest request, CancellationToken cancellationToken)
    {
        var transaction = _unitOfWork.BeginTransactionFor(request, _logger);

        try
        {
            if (request.Task == null)
            {
                throw new InvalidOperationException();
            }

            var pomodoro = new Pomodoro(request.Task, request.Timer, DateTime.Now);

            await _repository.Save(pomodoro);

            transaction.Commit();

            var response = new PomodoroCreationResponse(request.GetCorrelationId()) { };

            return response;
        }
        catch (Exception ex)
        {
            transaction.Rollback(ex);

            throw;
        }
    }
}
