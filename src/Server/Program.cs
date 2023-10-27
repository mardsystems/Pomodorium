using MongoDB.Driver;
using Pomodorium.Data;
using Pomodorium.Hubs;
using Pomodorium.Modules.Pomodori;
using System.DomainModel.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var readDatabaseConnectionString = builder.Configuration.GetConnectionString("ReadDatabase");

builder.Services.AddScoped(factory => new MongoClient(readDatabaseConnectionString));

var writeDatabaseConnectionString = builder.Configuration.GetConnectionString("WriteDatabase");

builder.Services.AddScoped<IAppendOnlyStore, MongoDBStore>(factory =>
    new MongoDBStore(
        new MongoClient(writeDatabaseConnectionString),
        factory.GetRequiredService<ILogger<MongoDBStore>>()));

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

builder.Services.AddSignalR();

builder.Services.AddScoped<PomodoroRepository>();

builder.Services.AddScoped<EventStore>();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(MongoDBPomodoroQueryItemsProjection).Assembly, typeof(PomodoroApplication).Assembly);
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
