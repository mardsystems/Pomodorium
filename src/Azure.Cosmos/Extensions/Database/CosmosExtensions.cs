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
        catch (Exception _)
        {

        }
    }

    public static void EnsureCreated(this CosmosClient cosmosClient, CosmosOptions options)
    {
        Microsoft.Azure.Cosmos.Database database;

        try
        {
            ThroughputProperties autoscaleThroughputProperties = ThroughputProperties.CreateAutoscaleThroughput(1000);

            var databaseResponse = cosmosClient.CreateDatabaseAsync(
                id: options.Database,
                throughputProperties: autoscaleThroughputProperties
            ).Result;

            database = databaseResponse.Database;
        }
        catch (AggregateException aggEx)
        {
            if (aggEx.InnerException is CosmosException cosmosException)
            {
                if (cosmosException.Message.Contains("Entity with the specified id already exists in the system"))
                {
                    database = cosmosClient.GetDatabase(
                        id: options.Database);
                }
                else
                {
                    database = null;
                }
            }
            else
            {
                database = null;
            }
        }

        database.CreateContainer("ActivityDetails");

        database.CreateContainer("ActivityQueryItems");

        database.CreateContainer("FlowtimeDetails");

        database.CreateContainer("FlowtimeQueryItems");

        database.CreateContainer("PomodoroDetails");

        database.CreateContainer("PomodoroQueryItems");

        database.CreateContainer("TfsIntegrationContainer");

        database.CreateContainer("TrelloIntegrationContainer");

        database.CreateContainer("EventStore");

        database.CreateContainer("TaskDetails");

        database.CreateContainer("TaskQueryItems");
    }

    private static void CreateContainer(
        this Microsoft.Azure.Cosmos.Database database,
        string name,
        string partitionKey = "/id")
    {
        try
        {
            database.CreateContainerAsync(
                id: name,
                partitionKeyPath: partitionKey
            ).Wait();
        }
        catch (AggregateException aggEx)
        {
            if (aggEx.InnerException is CosmosException cosmosException)
            {

            }
        }
    }
}
