using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using Pomodorium.Data;

namespace Pomodorium.Modules.Pomodori
{
    public class MongoDBPomodoriCommandHandler :
        IRequestHandler<GetPomodoriRequest, GetPomodoriResponse>,
        IRequestHandler<GetPomodoroRequest, GetPomodoroResponse>
    {
        private readonly MongoClient _mongoClient;

        public MongoDBPomodoriCommandHandler(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public async Task<GetPomodoriResponse> Handle(GetPomodoriRequest request, CancellationToken cancellationToken)
        {
            var collection = _mongoClient.GetDatabase("Pomodorium").GetCollection<PomodoroQueryItem>("PomodoroDetails");

            var filter = Builders<PomodoroQueryItem>.Filter.Empty;

            var pomodoroQueryItems = await collection.Find(filter).ToListAsync();

            var response = new GetPomodoriResponse(request.GetCorrelationId()) { PomodoroQueryItems = pomodoroQueryItems };

            return await Task.FromResult(response);
        }

        public async Task<GetPomodoroResponse> Handle(GetPomodoroRequest request, CancellationToken cancellationToken)
        {
            var collection = _mongoClient.GetDatabase("Pomodorium").GetCollection<PomodoroDetails>("PomodoroDetails");

            var filter = Builders<PomodoroDetails>.Filter.Eq(x => x.Id, request.Id);

            var pomodoroDetails = await collection.Find(filter).FirstAsync();

            if (pomodoroDetails == null)
            {
                //return NotFound();
            }

            var response = new GetPomodoroResponse(request.GetCorrelationId()) { PomodoroDetails = pomodoroDetails };

            return response;
        }
    }
}
