using MongoDB.Driver;
using Pomodorium.Data;
using Pomodorium.Features.ActivityManager;
using Pomodorium.Features.FlowTimer;
using Pomodorium.Hubs;
using Pomodorium.TeamFoundationServer;
using Pomodorium.Trello;
using RabbitMQ.Client;
using System.DomainModel;
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

builder.Services.AddScoped<Repository>();

builder.Services.AddScoped<EventStore>();

var connectionFactory = new ConnectionFactory()
{
    HostName = builder.Configuration["MessageBroker"]
};

//var connection = connectionFactory.CreateConnection();

//builder.Services.AddScoped((factory) => new RabbitMQPublisher(connection));

builder.Services.AddOptions<TeamFoundationServerOptions>()
    .Bind(builder.Configuration.GetSection(TeamFoundationServerOptions.NAME));

builder.Services.AddScoped<WorkItemAdapter>();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(
        typeof(Program).Assembly,
        typeof(CreateFlowtimeHandler).Assembly,
        typeof(MongoDBFlowtimeQueryItemsProjection).Assembly,
        typeof(PostActivityHandler).Assembly);
});

builder.Services.AddHttpClient(TrelloOptions.NAME, client =>
{
    client.BaseAddress = new Uri("https://api.trello.com/");
});


builder.Services.AddOptions<TrelloOptions>()
    .Bind(builder.Configuration.GetSection(TrelloOptions.NAME));

builder.Services.AddScoped<BoardsAdapter>();
builder.Services.AddScoped<ListsAdapter>();

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
