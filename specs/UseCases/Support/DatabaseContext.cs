using Microsoft.Azure.Cosmos;
using Pomodorium.Data;
using Pomodorium.Extensions.Database;

namespace Pomodorium.Support;

public class DatabaseContext
{
    public CosmosClient Client { get; }

    public CosmosOptions Options { get; }

    public DatabaseContext(CosmosClient client, CosmosOptions options)
    {
        Client = client;

        Options = options;
    }

    public void EnsureCreated()
    {
        Client.EnsureCreated(Options);
    }

    public void EnsureDeleted()
    {
        Client.EnsureDeleted(Options);
    }
}
