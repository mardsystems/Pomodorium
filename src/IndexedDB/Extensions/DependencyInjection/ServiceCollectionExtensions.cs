using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomodorium.Data;
using Pomodorium.Features.Storage;
using System.DomainModel.Storage;
using System.Reflection;

namespace Pomodorium.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIndexedDB(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<IndexedDBOptions>()
            .Bind(configuration.GetSection(IndexedDBOptions.CONFIGURATION_SECTION_NAME));

        services.AddScoped<IndexedDBAccessor>();

        services.AddScoped<IAppendOnlyStore, IndexedDBStore>();

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        return services;
    }
}
