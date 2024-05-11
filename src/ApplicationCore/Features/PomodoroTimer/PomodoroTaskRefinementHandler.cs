using Microsoft.Extensions.Logging;
using Pomodorium.Models.Pomos;
using System.ApplicationModel;

namespace Pomodorium.Features.PomodoroTimer;

public class PomodoroTaskRefinementHandler : IRequestHandler<PomodoroTaskRefinementRequest, PomodoroTaskRefinementResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly Repository _repository;

    private readonly ILogger<PomodoroTaskRefinementHandler> _logger;
    
    public PomodoroTaskRefinementHandler(IUnitOfWork unitOfWork, Repository pomodoroRepository, ILogger<PomodoroTaskRefinementHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _repository = pomodoroRepository;
        _logger = logger;
    }

    public async Task<PomodoroTaskRefinementResponse> Handle(PomodoroTaskRefinementRequest request, CancellationToken cancellationToken)
    {
        var transaction = _unitOfWork.BeginTransactionFor(request, _logger);

        try
        {
            var pomodoro = await _repository.GetAggregateById<Pomodoro>(request.Id) ?? throw new EntityNotFoundException();

            pomodoro.RefineTask(request.Task);

            await _repository.Save(pomodoro, request.Version);

            transaction.Commit();

            var response = new PomodoroTaskRefinementResponse(request.GetCorrelationId());

            return response;
        }
        catch (Exception ex)
        {
            transaction.Rollback(ex);

            throw;
        }
    }
}
