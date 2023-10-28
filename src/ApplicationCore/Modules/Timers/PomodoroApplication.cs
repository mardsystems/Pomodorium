using MediatR;
using System.DomainModel;

namespace Pomodorium.Modules.Timers
{
    public class PomodoroApplication :
        IRequestHandler<PostPomodoroRequest, PostPomodoroResponse>,
        IRequestHandler<PutPomodoroRequest, PutPomodoroResponse>,
        IRequestHandler<DeletePomodoroRequest, DeletePomodoroResponse>
    {
        private readonly TimersRepository _pomodoroRepository;

        public PomodoroApplication(TimersRepository pomodoroRepository)
        {
            _pomodoroRepository = pomodoroRepository;
        }

        public async Task<PostPomodoroResponse> Handle(PostPomodoroRequest request, CancellationToken cancellationToken)
        {
            var correlationId = request.GetCorrelationId();

            var pomodoroId = new PomodoroId(correlationId);

            var pomodoro = new Pomodoro(pomodoroId, request.StartDateTime, request.Description);

            await _pomodoroRepository.Save(pomodoro, -1);

            var response = new PostPomodoroResponse(request.GetCorrelationId()) { };

            return response;
        }

        public async Task<PutPomodoroResponse> Handle(PutPomodoroRequest request, CancellationToken cancellationToken)
        {
            var pomodoroId = new PomodoroId(request.Id);

            var pomodoro = await _pomodoroRepository.GetPomodoroById(pomodoroId);

            if (pomodoro == null)
            {
                throw new EntityNotFoundException();
            }

            pomodoro.ChangeDescription(request.Description);

            await _pomodoroRepository.Save(pomodoro, request.Version);

            var response = new PutPomodoroResponse(request.GetCorrelationId()) { };

            return response;
        }

        public async Task<DeletePomodoroResponse> Handle(DeletePomodoroRequest request, CancellationToken cancellationToken)
        {
            var pomodoroId = new PomodoroId(request.Id);

            var pomodoro = await _pomodoroRepository.GetPomodoroById(pomodoroId);

            if (pomodoro == null)
            {
                throw new EntityNotFoundException();
            }

            pomodoro.Archive();

            await _pomodoroRepository.Save(pomodoro, request.Version);

            var response = new DeletePomodoroResponse(request.GetCorrelationId()) { };

            return response;
        }
    }
}
