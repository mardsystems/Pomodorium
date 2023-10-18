using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Pomodorium;
using Pomodorium.Data;
using Pomodorium.Handlers;
using Pomodorium.Hubs;
using Pomodorium.Modules.Pomodori;
using System.DomainModel.EventStore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IAppendOnlyStore>(factory => new MemoryStore(@"Data Source=EventStore.db"));

var connectionString = builder.Configuration.GetConnectionString("MONGODB_URI");

builder.Services.AddScoped(factory => new MongoClient(connectionString));

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

builder.Services.AddSignalR();

builder.Services.AddScoped<PomodoroRepository>();

builder.Services.AddScoped<EventStoreRepository>();

builder.Services.AddScoped<IAppendOnlyStore, MongoDBStore>();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(MongoDBPomodoriEventHandler).Assembly);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();

app.MapHub<EventHub>("/events");

app.MapFallbackToFile("index.html");

app.Run();
