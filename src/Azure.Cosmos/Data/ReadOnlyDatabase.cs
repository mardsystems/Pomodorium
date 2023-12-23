using Microsoft.Extensions.Options;
using Pomodorium.Extensions.Database;
using Pomodorium.Features.Maintenance;

namespace Pomodorium.Data;

internal class ReadOnlyDatabase : IReadOnlyDatabase
{
    private readonly CosmosClient _cosmosClient;

    private readonly CosmosOptions _options;

    public ReadOnlyDatabase(CosmosClient cosmosClient, IOptions<CosmosOptions> cosmosOptionsInerface)
    {
        _cosmosClient = cosmosClient;
        _options = cosmosOptionsInerface.Value;
    }

    public async Task EnsureCreated()
    {
        Database database;

        ThroughputProperties autoscaleThroughputProperties = ThroughputProperties.CreateAutoscaleThroughput(1000);

        var databaseResponse = _cosmosClient.CreateDatabaseIfNotExistsAsync(
            id: _options.Database,
            throughputProperties: autoscaleThroughputProperties
        ).Result;

        database = databaseResponse.Database;

        // Read database.

        database.CreateContainerIfNotExists("ActivityDetails");
        database.CreateContainerIfNotExists("ActivityQueryItems");
        database.CreateContainerIfNotExists("FlowtimeDetails");
        database.CreateContainerIfNotExists("FlowtimeQueryItems");
        database.CreateContainerIfNotExists("PomodoroDetails");
        database.CreateContainerIfNotExists("PomodoroQueryItems");
        database.CreateContainerIfNotExists("TaskDetails");
        database.CreateContainerIfNotExists("TaskQueryItems");

        await Task.CompletedTask;
    }

    public async Task EnsureDeleted()
    {
        try
        {
            var database = _cosmosClient.GetDatabase(
                id: _options.Database);

            await database.DeleteContainerAsync("ActivityDetails");
            await database.DeleteContainerAsync("ActivityQueryItems");
            await database.DeleteContainerAsync("FlowtimeDetails");
            await database.DeleteContainerAsync("FlowtimeQueryItems");
            await database.DeleteContainerAsync("PomodoroDetails");
            await database.DeleteContainerAsync("PomodoroQueryItems");
            await database.DeleteContainerAsync("TaskDetails");
            await database.DeleteContainerAsync("TaskQueryItems");
        }
        catch (AggregateException aggEx)
        {
            if (aggEx.InnerException is CosmosException cosmosException)
            {
                if (cosmosException.Message.Contains("Resource Not Found"))
                {
                    // OK.
                }
                else
                {
                    throw;
                }
            }
            else
            {
                throw;
            }
        }
    }
}
