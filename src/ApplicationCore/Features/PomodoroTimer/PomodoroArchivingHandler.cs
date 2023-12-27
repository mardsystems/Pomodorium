using Microsoft.Extensions.Logging;
using Pomodorium.Models.PomodoroTechnique;
using System.ApplicationModel;

namespace Pomodorium.Features.PomodoroTimer;

public class PomodoroArchivingHandler : IRequestHandler<PomodoroArchivingRequest, PomodoroArchivingResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly Repository _repository;

    private readonly ILogger<PomodoroArchivingHandler> _logger;
    
    public PomodoroArchivingHandler(IUnitOfWork unitOfWork, Repository pomodoroRepository, ILogger<PomodoroArchivingHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _repository = pomodoroRepository;
        _logger = logger;
    }

    public async Task<PomodoroArchivingResponse> Handle(PomodoroArchivingRequest request, CancellationToken cancellationToken)
    {
        var transaction = _unitOfWork.BeginTransactionFor(request, _logger);

        try
        {
            var pomodoro = await _repository.GetAggregateById<Pomodoro>(request.Id) ?? throw new EntityNotFoundException();

            pomodoro.Archive();

            await _repository.Save(pomodoro, request.Version);

            transaction.Commit();

            var response = new PomodoroArchivingResponse(request.GetCorrelationId()) { };

            return response;
        }
        catch (Exception ex)
        {
            transaction.Rollback(ex);

            throw;
        }
    }
}
