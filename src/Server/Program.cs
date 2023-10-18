using Pomodorium;
using Pomodorium.Handlers;
using Pomodorium.Hubs;
using System.DomainModel.EventStore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddSignalR();

builder.Services.AddScoped<EventStoreRepository>();

builder.Services.AddSingleton<IAppendOnlyStore>(factory => new MemoryStore(@"Data Source=EventStore.db"));

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(EventHubHandler).Assembly);
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
