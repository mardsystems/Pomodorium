using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Pomodorium.Data;
using Pomodorium.Features.Settings;
using Pomodorium.Features.Storage;
using System.DomainModel.Storage;
using System.Reflection;

namespace Pomodorium.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoDB(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<MongoDBOptions>()
            .Bind(configuration.GetSection(MongoDBOptions.CONFIGURATION_SECTION_NAME));

        var readDatabaseConnectionString = configuration.GetConnectionString("ReadDatabase");

        services.AddScoped(factory => new MongoClient(readDatabaseConnectionString));

        var writeDatabaseConnectionString = configuration.GetConnectionString("WriteDatabase");

        services.AddScoped<IAppendOnlyStore, MongoDBStore>(factory =>
            new MongoDBStore(
                new MongoClient(writeDatabaseConnectionString),
                factory.GetRequiredService<IOptions<MongoDBOptions>>(),
                factory.GetRequiredService<ILogger<MongoDBStore>>()));

        services.AddScoped<MongoDBTfsIntegrationCollection>();

        services.AddScoped<MongoDBTrelloIntegrationCollection>();

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        return services;
    }
}
