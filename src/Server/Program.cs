using MongoDB.Driver;
using Pomodorium.Features.ActivityManager;
using Pomodorium.Features.FlowTimer;
using Pomodorium.Features.Settings;
using Pomodorium.Features.Storage;
using Pomodorium.Features.TaskSynchronizer;
using Pomodorium.Handlers;
using Pomodorium.Hubs;
using Pomodorium.Integrations.TFS;
using Pomodorium.Integrations.Trello;
using RabbitMQ.Client;
using System.DomainModel;
using System.DomainModel.Storage;

namespace Pomodorium;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        var readDatabaseConnectionString = builder.Configuration.GetConnectionString("ReadDatabase");

        builder.Services.AddScoped(factory => new MongoClient(readDatabaseConnectionString));

        var writeDatabaseConnectionString = builder.Configuration.GetConnectionString("WriteDatabase");

        builder.Services.AddScoped<IAppendOnlyStore, MongoDBStore>(factory =>
            new MongoDBStore(
                new MongoClient(writeDatabaseConnectionString),
                factory.GetRequiredService<ILogger<MongoDBStore>>()));

        builder.Services.AddScoped<MongoDBTfsIntegrationCollection>();

        builder.Services.AddScoped<MongoDBTrelloIntegrationCollection>();

        builder.Services.AddControllersWithViews();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();

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

        builder.Services.AddOptions<TfsIntegrationOptions>()
            .Bind(builder.Configuration.GetSection(TfsIntegrationOptions.CONFIGURATION_SECTION_NAME));

        builder.Services.AddScoped<WorkItemAdapter>();

        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(
                typeof(Program).Assembly,
                typeof(MongoDBEventHandler).Assembly,
                typeof(CreateFlowtimeHandler).Assembly,
                typeof(MongoDBFlowtimeQueryItemsProjection).Assembly,
                typeof(PostActivityHandler).Assembly);
        });

        var trelloConfigurationSection = builder.Configuration.GetSection(TrelloIntegrationOptions.CONFIGURATION_SECTION_NAME);

        var trelloIntegrationOptions = trelloConfigurationSection.Get<TrelloIntegrationOptions>();

        builder.Services.AddOptions<TrelloIntegrationOptions>()
            .Bind(trelloConfigurationSection);

        builder.Services.AddHttpClient(TrelloIntegrationOptions.CONFIGURATION_SECTION_NAME, client =>
        {
            client.BaseAddress = new Uri(trelloIntegrationOptions.BaseAddress);
        });

        builder.Services.AddScoped<CardAdapter>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();

            app.UseSwagger();

            app.UseSwaggerUI();
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
    }
}
