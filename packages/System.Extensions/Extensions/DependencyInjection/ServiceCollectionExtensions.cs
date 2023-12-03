using Microsoft.Extensions.DependencyInjection;
using System.DomainModel;
using System.DomainModel.Storage;

namespace System.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSystem(this IServiceCollection services)
    {
        services.AddScoped<Repository>();

        services.AddScoped<EventStore>();

        return services;
    }
}
