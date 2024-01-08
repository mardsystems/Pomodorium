using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor.Services;
using Pomodorium;
using Pomodorium.Data;
using Pomodorium.Extensions.DependencyInjection;
using Pomodorium.Hubs;
using System.Extensions.DependencyInjection;
using System.Reflection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var APP_REMOTE = true;

builder.Services.AddSystem();

if (APP_REMOTE)
{
    builder.Services.AddApplicationRemote();

    builder.Services.AddSharedInfrastructure();
}
else
{
    builder.Services.AddTaskManagementCore();
    
    builder.Services.AddApplicationCore();

    builder.Services.AddClientInfrastructure(builder.Configuration);

    builder.Services.AddScoped<EventHubClient>();
}

builder.Services.AddLocalization();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("Pomodorium.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(factory => factory.GetRequiredService<IHttpClientFactory>().CreateClient("Pomodorium.ServerAPI"));

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);

    var scopes = builder.Configuration.GetSection("ServerApi")["Scopes"] ?? throw new InvalidOperationException();

    options.ProviderOptions.DefaultAccessTokenScopes.Add(scopes);
});

var hubConnectionBuilder = new HubConnectionBuilder();

var hubConnection = hubConnectionBuilder.WithUrl(new Uri($"{builder.HostEnvironment.BaseAddress}events")).Build();

builder.Services.AddScoped(sp => hubConnection);

builder.Services.AddMudServices();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});

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
