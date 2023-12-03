using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Pomodorium.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationRemote(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        return services;
    }
}
