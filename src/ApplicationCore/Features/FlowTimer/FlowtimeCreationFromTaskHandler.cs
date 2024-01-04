using Microsoft.Extensions.Logging;
using Pomodorium.Features.TaskManager;
using Pomodorium.Models.FlowtimeTechnique;
using System.ApplicationModel;

namespace Pomodorium.Features.FlowTimer;

public class FlowtimeCreationFromTaskHandler : IRequestHandler<FlowtimeCreationFromTaskRequest, FlowtimeCreationFromTaskResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly Repository _repository;

    private readonly ILogger<TaskRegistrationHandler> _logger;

    public FlowtimeCreationFromTaskHandler(IUnitOfWork unitOfWork, Repository repository, ILogger<TaskRegistrationHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _logger = logger;
    }

    public async Task<FlowtimeCreationFromTaskResponse> Handle(FlowtimeCreationFromTaskRequest request, CancellationToken cancellationToken)
    {
        var transaction = _unitOfWork.BeginTransactionFor(request, _logger);

        try
        {
            var task = await _repository.GetAggregateById<Models.TaskManagement.Tasks.Task>(request.TaskId);

            if (task.Description != request.TaskDescription)
            {
                task.ChangeDescription(request.TaskDescription);

                await _repository.Save(task, request.TaskVersion ?? -1);

                task = await _repository.GetAggregateById<Models.TaskManagement.Tasks.Task>(request.TaskId);
            }

            var flowtimeId = Guid.NewGuid();

            var flowtime = new Flowtime(flowtimeId, task, transaction);

            await _repository.Save(flowtime);

            transaction.Commit();

            var response = new FlowtimeCreationFromTaskResponse(request.GetCorrelationId())
            {
                FlowtimeId = flowtime.Id,
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
