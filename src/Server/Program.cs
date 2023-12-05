using Pomodorium.Extensions.DependencyInjection;
using Pomodorium.Extensions.Infrastructure;
using Pomodorium.Hubs;
using System.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web.UI;

namespace Pomodorium;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

        //builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
        //    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

        builder.Services.AddSystem();

        builder.Services.AddApplicationCore();

        builder.Services.AddServerInfrastructure(builder.Configuration);

        builder.Services.AddControllersWithViews();

        //builder.Services.AddControllersWithViews(options =>
        //{
        //    var policy = new AuthorizationPolicyBuilder()
        //        .RequireAuthenticatedUser()
        //        .Build();
        //    options.Filters.Add(new AuthorizeFilter(policy));
        //});

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();

        builder.Services.AddRazorPages();
            //.AddMicrosoftIdentityUI();

        builder.Services.AddSignalR();

        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        var app = builder.Build();

        app.UseServerInfrastructure();

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

        //app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();
        app.MapControllers();

        app.MapHub<EventHub>("/events");

        app.MapFallbackToFile("index.html");

        app.Run();
    }
}
