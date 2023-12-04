using Microsoft.Extensions.Hosting;
using Pomodorium.Extensions.Database;

namespace Pomodorium.Extensions.Infrastructure;

public static class HostExtensions
{
    public static void UseServerInfrastructure(this IHost host)
    {
        host.EnsureCreateCosmosDatabase();
    }
}
