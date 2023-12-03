using Pomodorium.Extensions.DependencyInjection;
using Pomodorium.Hubs;
using System.Extensions.DependencyInjection;
using System.Reflection;

namespace Pomodorium;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSystem();

        builder.Services.AddApplicationCore();

        builder.Services.AddServerInfrastructure(builder.Configuration);

        builder.Services.AddControllersWithViews();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();

        builder.Services.AddRazorPages();

        builder.Services.AddSignalR();

        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

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
