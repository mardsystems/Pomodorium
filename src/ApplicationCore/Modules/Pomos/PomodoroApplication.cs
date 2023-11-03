using MediatR;
using System.DomainModel;

namespace Pomodorium.Modules.Pomos;

public class PomodoroApplication :
    IRequestHandler<PostPomodoroRequest, PostPomodoroResponse>,
    IRequestHandler<PutPomodoroRequest, PutPomodoroResponse>,
    IRequestHandler<DeletePomodoroRequest, DeletePomodoroResponse>
{
    private readonly Repository _repository;

    public PomodoroApplication(Repository pomodoroRepository)
    {
        _repository = pomodoroRepository;
    }

    public async Task<PostPomodoroResponse> Handle(PostPomodoroRequest request, CancellationToken cancellationToken)
    {
        var correlationId = request.GetCorrelationId();

        var pomodoroId = correlationId;

        var pomodoro = new Pomodoro(pomodoroId, request.Description);

        await _repository.Save(pomodoro, -1);

        var response = new PostPomodoroResponse(request.GetCorrelationId()) { };

        return response;
    }

    public async Task<PutPomodoroResponse> Handle(PutPomodoroRequest request, CancellationToken cancellationToken)
    {
        var pomodoro = await _repository.GetAggregateById<Pomodoro>(request.Id);

        if (pomodoro == null)
        {
            throw new EntityNotFoundException();
        }

        pomodoro.ChangeDescription(request.Description);

        await _repository.Save(pomodoro, request.Version);

        var response = new PutPomodoroResponse(request.GetCorrelationId()) { };

        return response;
    }

    public async Task<DeletePomodoroResponse> Handle(DeletePomodoroRequest request, CancellationToken cancellationToken)
    {
        var pomodoro = await _repository.GetAggregateById<Pomodoro>(request.Id);

        if (pomodoro == null)
        {
            throw new EntityNotFoundException();
        }

        pomodoro.Archive();

        await _repository.Save(pomodoro, request.Version);

        var response = new DeletePomodoroResponse(request.GetCorrelationId()) { };

        return response;
    }
}
