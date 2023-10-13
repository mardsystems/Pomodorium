using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Pomodorium;
using Pomodorium.Data;
using Pomodorium.Modules.Pomodori;
using System.DomainModel.EventStore;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddLocalization();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//builder.Services.AddScoped(sp =>
//{
//    var hubConnectionBuilder = new HubConnectionBuilder();

//    var connection = hubConnectionBuilder.WithUrl(new Uri($"{builder.HostEnvironment.BaseAddress}/events" )).Build();

//    return connection;
//});

builder.Services.AddScoped<PomodoroQueryDbService>();

builder.Services.AddScoped<PomodoroRepository>();

builder.Services.AddScoped<EventStoreRepository>();

builder.Services.AddSingleton<IAppendOnlyStore>(factory => new MemoryStore(@"Data Source=EventStore.db"));

builder.Services.AddSingleton<PomodoriumDbContext>();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(PomodoroEventHandler).Assembly);
});

await builder.Build().RunAsync();
