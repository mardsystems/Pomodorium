using Pomodorium;
using Pomodorium.Data;
using Pomodorium.Handlers;
using Pomodorium.Hubs;
using Pomodorium.Modules.Pomodori;
using System.DomainModel.EventStore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddSignalR();

builder.Services.AddScoped<PomodoroQueryDbService>();

builder.Services.AddScoped<PomodoroRepository>();

builder.Services.AddScoped<EventStoreRepository>();

builder.Services.AddSingleton<IAppendOnlyStore>(factory => new MemoryStore(@"Data Source=EventStore.db"));

builder.Services.AddSingleton<PomodoriumDbContext>();

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
