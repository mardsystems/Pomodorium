using MediatR;

namespace Pomodorium.Modules.Pomodori
{
    public class DomainPomodoriCommandHandler : 
        IRequestHandler<PostPomodoroRequest, PostPomodoroResponse>,
        IRequestHandler<PutPomodoroRequest, PutPomodoroResponse>,
        IRequestHandler<DeletePomodoroRequest, DeletePomodoroResponse>
    {
        private readonly PomodoroRepository _pomodoroRepository;

        public DomainPomodoriCommandHandler(PomodoroRepository pomodoroRepository)
        {
            _pomodoroRepository = pomodoroRepository;
        }

        public async Task<PostPomodoroResponse> Handle(PostPomodoroRequest request, CancellationToken cancellationToken)
        {
            var correlationId = request.GetCorrelationId();

            var pomodoroId = new PomodoroId(correlationId.ToString());

            var pomodoro = new Pomodoro(pomodoroId, request.StartDateTime, request.Description);

            await _pomodoroRepository.Add(pomodoro);

            var response = new PostPomodoroResponse(request.GetCorrelationId()) { };

            return response;
        }

        public async Task<PutPomodoroResponse> Handle(PutPomodoroRequest request, CancellationToken cancellationToken)
        {
            var pomodoroId = new PomodoroId(request.Id);

            var pomodoro = await _pomodoroRepository.GetPomodoroById(pomodoroId);

            //if (pomodoro == null)
            //{
            //    return NotFound();
            //}

            pomodoro.ChangeDescription(request.Description);

            await _pomodoroRepository.Update(pomodoro);

            var response = new PutPomodoroResponse(request.GetCorrelationId()) { };

            return response;
        }

        public async Task<DeletePomodoroResponse> Handle(DeletePomodoroRequest request, CancellationToken cancellationToken)
        {
            var pomodoroId = new PomodoroId(request.Id);

            var pomodoro = await _pomodoroRepository.GetPomodoroById(pomodoroId);

            //if (pomodoro == null)
            //{
            //    return NotFound();
            //}

            //await _pomodoroRepository.Delete(pomodoro);

            var response = new DeletePomodoroResponse(request.GetCorrelationId()) {  };

            return response;
        }
    }
}
