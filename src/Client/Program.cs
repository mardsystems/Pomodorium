using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Pomodorium;
using Pomodorium.Data;
using Pomodorium.Handlers;
using Pomodorium.Hubs;
using Pomodorium.Modules.Pomodori;
using System.DomainModel.Storage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddLocalization();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<IndexedDBAccessor>();

builder.Services.AddScoped<IAppendOnlyStore, IndexedDBStore>();

builder.Services.AddScoped(sp =>
{
    var hubConnectionBuilder = new HubConnectionBuilder();

    var connection = hubConnectionBuilder.WithUrl(new Uri($"{builder.HostEnvironment.BaseAddress}events")).Build();

    return connection;
});

builder.Services.AddScoped<EventHubClient>();

builder.Services.AddScoped<PomodoroRepository>();

builder.Services.AddScoped<EventStore>();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(EventHubHandler).Assembly, typeof(PomodoroApplication).Assembly);
});

var host = builder.Build();

using var scope = host.Services.CreateScope();
await using var indexedDB = scope.ServiceProvider.GetService<IndexedDBAccessor>();

if (indexedDB is not null)
{
    await indexedDB.InitializeAsync();
}

await host.RunAsync();
