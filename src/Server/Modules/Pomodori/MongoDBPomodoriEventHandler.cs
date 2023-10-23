using MediatR;
using MongoDB.Driver;

namespace Pomodorium.Modules.Pomodori;

public class MongoDBPomodoriEventHandler :
    INotificationHandler<PomodoroCreated>
{
    private readonly MongoClient _mongoClient;

    public MongoDBPomodoriEventHandler(MongoClient mongoClient)
    {
        _mongoClient = mongoClient;
    }

    public async Task Handle(PomodoroCreated request, CancellationToken cancellationToken)
    {
        var pomodoroQueryItem = new PomodoroQueryItem
        {
            Id = request.Id.ToString(),
            StartDateTime = request.StartDateTime,
            Description = request.Description
        };

        var collection = _mongoClient.GetDatabase("Pomodorium").GetCollection<PomodoroQueryItem>("PomodoroDetails");

        await collection.InsertOneAsync(pomodoroQueryItem);
    }
}
