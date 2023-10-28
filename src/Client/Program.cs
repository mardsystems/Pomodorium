using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Pomodorium;
using Pomodorium.Data;
using Pomodorium.Handlers;
using Pomodorium.Hubs;
using Pomodorium.Modules.Timers;
using System.DomainModel.Storage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var APP_REMOTE = true;

builder.Services.AddLocalization();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

if (APP_REMOTE)
{
    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

    builder.Services.AddMediatR(config =>
    {
        config.RegisterServicesFromAssembly(typeof(HttpClientTimersFacade).Assembly);
    });
}
else
{
    builder.Services.AddScoped(sp =>
    {
        var hubConnectionBuilder = new HubConnectionBuilder();

        var connection = hubConnectionBuilder.WithUrl(new Uri($"{builder.HostEnvironment.BaseAddress}events")).Build();

        return connection;
    });

    builder.Services.AddScoped<IndexedDBAccessor>();

    builder.Services.AddScoped<IAppendOnlyStore, IndexedDBStore>();

    builder.Services.AddScoped<EventHubClient>();

    builder.Services.AddScoped<TimersRepository>();

    builder.Services.AddScoped<EventStore>();

    builder.Services.AddMediatR(config =>
    {
        config.RegisterServicesFromAssemblies(typeof(Program).Assembly, typeof(PomodoroApplication).Assembly);
    });
}

var host = builder.Build();

if (!APP_REMOTE)
{
    await using var scope = host.Services.CreateAsyncScope();

    await using var indexedDB = scope.ServiceProvider.GetService<IndexedDBAccessor>();

    if (indexedDB is not null)
    {
        await indexedDB.InitializeAsync();
    }
}

await host.RunAsync();
