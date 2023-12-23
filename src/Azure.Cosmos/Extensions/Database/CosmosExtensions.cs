using Pomodorium.Data;

namespace Pomodorium.Extensions.Database;

public static class CosmosExtensions
{
    public static void EnsureDeleted(this CosmosClient cosmosClient, CosmosOptions options)
    {
        try
        {
            var database = cosmosClient.GetDatabase(
                id: options.Database);

            database.DeleteAsync().Wait();
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

    public static void EnsureCreated(this CosmosClient cosmosClient, CosmosOptions options)
    {
        Microsoft.Azure.Cosmos.Database database;

        ThroughputProperties autoscaleThroughputProperties = ThroughputProperties.CreateAutoscaleThroughput(1000);

        var databaseResponse = cosmosClient.CreateDatabaseIfNotExistsAsync(
            id: options.Database,
            throughputProperties: autoscaleThroughputProperties
        ).Result;

        database = databaseResponse.Database;

        // Write database.

        database.CreateContainerIfNotExists("EventStore");

        // Database.

        database.CreateContainerIfNotExists("TfsIntegrationContainer");
        database.CreateContainerIfNotExists("TrelloIntegrationContainer");

        // Read database.

        database.CreateContainerIfNotExists("ActivityDetails");
        database.CreateContainerIfNotExists("ActivityQueryItems");
        database.CreateContainerIfNotExists("FlowtimeDetails");
        database.CreateContainerIfNotExists("FlowtimeQueryItems");
        database.CreateContainerIfNotExists("PomodoroDetails");
        database.CreateContainerIfNotExists("PomodoroQueryItems");
        database.CreateContainerIfNotExists("TaskDetails");
        database.CreateContainerIfNotExists("TaskQueryItems");
    }

    internal static void CreateContainerIfNotExists(
        this Microsoft.Azure.Cosmos.Database database,
        string name,
        string partitionKey = "/id")
    {
        database.CreateContainerIfNotExistsAsync(
            id: name,
            partitionKeyPath: partitionKey
        ).Wait();
    }

    internal static async Task DeleteContainerAsync(
        this Microsoft.Azure.Cosmos.Database database,
        string name)
    {
        try
        {
            var container = database.GetContainer(name);

            await container.DeleteContainerAsync();
        }
        catch (Exception)
        {
            // Fails silently.
        }
    }
}
