using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Pomodorium;
using Pomodorium.Data;
using Pomodorium.Hubs;
using Pomodorium.Modules.Pomos;
using System.DomainModel;
using System.DomainModel.Storage;
using static System.Formats.Asn1.AsnWriter;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var APP_REMOTE = false;

builder.Services.AddLocalization();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var hubConnectionBuilder = new HubConnectionBuilder();

var hubConnection = hubConnectionBuilder.WithUrl(new Uri($"{builder.HostEnvironment.BaseAddress}events")).Build();

builder.Services.AddScoped(sp => hubConnection);

if (APP_REMOTE)
{
    builder.Services.AddMediatR(config =>
    {
        config.RegisterServicesFromAssembly(typeof(HttpClientPomosFacade).Assembly);
    });
}
else
{
    builder.Services.AddScoped<EventHubClient>();

    builder.Services.AddScoped<IndexedDBAccessor>();

    builder.Services.AddScoped<IAppendOnlyStore, IndexedDBStore>();

    builder.Services.AddScoped<Repository>();

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
    
    await hubConnection.StartAsync();
}

await host.RunAsync();
