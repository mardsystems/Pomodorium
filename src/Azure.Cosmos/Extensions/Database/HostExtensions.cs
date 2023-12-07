using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pomodorium.Data;

namespace Pomodorium.Extensions.Database;

public static class HostExtensions
{
    public static void EnsureCreateCosmosDatabase(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var cosmosClient = services.GetRequiredService<CosmosClient>();

                var cosmosOptionsInterface = services.GetService<IOptions<CosmosOptions>>();

                cosmosClient.EnsureCreated(cosmosOptionsInterface.Value);
            }
            catch (Exception ex)
            {
                //var logger = scope.ServiceProvider.GetRequiredService<ILogger>();

                //logger.LogError(ex, "An error occurred while migrating or seeding the database.");

                throw;
            }
        }
    }
}
