using MediatR;
using Pomodorium.Data;

namespace Pomodorium.Modules.Pomodori
{
    public class MongoDBPomodoriCommandHandler : 
        IRequestHandler<GetPomodoriRequest, GetPomodoriResponse>,
        IRequestHandler<GetPomodoroRequest, GetPomodoroResponse>
    {
        private readonly PomodoriumDbContext _db;

        public MongoDBPomodoriCommandHandler(PomodoriumDbContext db)
        {
            _db = db;
        }

        public async Task<GetPomodoriResponse> Handle(GetPomodoriRequest request, CancellationToken cancellationToken)
        {
            var pomodoroQueryItems = _db.PomodoroQueryItems
                .ToArray();

            var response = new GetPomodoriResponse(request.GetCorrelationId()) { PomodoroQueryItems = pomodoroQueryItems };

            return await Task.FromResult(response);
        }

        public async Task<GetPomodoroResponse> Handle(GetPomodoroRequest request, CancellationToken cancellationToken)
        {
            var pomodoroDetails = _db.PomodoroDetails
                .FirstOrDefault(x => x.Id == request.Id);

            if (pomodoroDetails == null)
            {
                //return NotFound();
            }

            var response = new GetPomodoroResponse(request.GetCorrelationId()) { PomodoroDetails = pomodoroDetails };

            return await Task.FromResult(response);
        }
    }
}
