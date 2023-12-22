using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Pomodorium.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionFactory = new ConnectionFactory()
        {
            HostName = configuration["MessageBroker"]
        };

        //var connection = connectionFactory.CreateConnection();

        //builder.Services.AddScoped((factory) => new RabbitMQPublisher(connection));

        return services;
    }
}
