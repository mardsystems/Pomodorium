﻿using Pomodorium.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System.Data.Common;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Pomodorium.Support;

public class AppHostingContext : IDisposable
{
    class TestAppFactory : WebApplicationFactory<Program>
    {
        private IServiceScope _serviceScope;

        //private PomodoriumContext _db;

        //public PomodoriumContext GetDbContext() => _db;

        protected override IHost CreateHost(IHostBuilder builder)
        {
            var host = builder.Build();

            _serviceScope = host.Services.CreateScope();

            var services = _serviceScope.ServiceProvider;

            try
            {
                //_db = services.GetRequiredService<PomodoriumContext>();

                //_db.Database.EnsureCreated();
            }
            catch (Exception _)
            {
                var logger = _serviceScope.ServiceProvider.GetRequiredService<ILogger<AppHostingContext>>();

                Console.WriteLine("An error occurred while migrating or seeding the database.");

                throw;
            }

            host.Start();

            return host;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureServices(services =>
            {
                //var dbContextDescriptor = services.SingleOrDefault(d =>
                //    d.ServiceType == typeof(DbContextOptions<PomodoriumContext>));

                //services.Remove(dbContextDescriptor);

                //var dbConnectionDescriptor = services.SingleOrDefault(d =>
                //    d.ServiceType == typeof(DbConnection));

                //services.Remove(dbConnectionDescriptor);

                //// Create open SqliteConnection so EF won't automatically close it.
                //services.AddSingleton<DbConnection>(container =>
                //{
                //    var connection = new SqliteConnection("DataSource=:memory:");

                //    connection.Open();

                //    return connection;
                //});

                //services.AddDbContext<PomodoriumContext>((container, options) =>
                //{
                //    var connection = container.GetRequiredService<DbConnection>();

                //    options.UseSqlite(connection);

                //    //options.UseInMemoryDatabase("InMemoryDbForTesting");
                //});
            });

            builder
                .ConfigureTestServices(services =>
                {
                    services.AddAuthentication(authentication =>
                    {
                        authentication.DefaultAuthenticateScheme = "TestScheme";
                        authentication.DefaultChallengeScheme = "TestScheme";
                    })
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                            "TestScheme", options => { });
                });

            builder.ConfigureLogging((context, loggingBuilder) =>
            {
                loggingBuilder.ClearProviders();

                loggingBuilder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, DebugLoggerProvider>());
            });

            builder.UseEnvironment("Development");
        }

        protected override void Dispose(bool disposing)
        {
            if (_serviceScope != null)
            {
                _serviceScope.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Debug Logger
        [ProviderAlias("Debug")]
        class DebugLoggerProvider : ILoggerProvider
        {
            public ILogger CreateLogger(string name) => new DebugLogger(name);

            public void Dispose()
            {
            }
        }

        class DebugLogger : ILogger
        {
            private readonly string _name;

            public DebugLogger(string name)
            {
                _name = string.IsNullOrEmpty(name) ? nameof(DebugLogger) : name;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return NoopDisposable.Instance;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return Debugger.IsAttached && logLevel != LogLevel.None;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                if (!IsEnabled(logLevel))
                    return;

                var message = formatter(state, exception);
                if (string.IsNullOrEmpty(message))
                    return;

                message = $"{logLevel}: {message}";

                if (exception != null)
                    message += Environment.NewLine + Environment.NewLine + exception;

                Debug.WriteLine(message, _name);
            }

            private class NoopDisposable : IDisposable
            {
                public static readonly NoopDisposable Instance = new NoopDisposable();

                public void Dispose()
                {
                }
            }
        }
        #endregion
    }

    private static TestAppFactory? _webApplicationFactory;

    public HttpClient CreateHttpClient()
    {
        _webApplicationFactory.Should().NotBeNull("the app should be running");

        var options = new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"),
            AllowAutoRedirect = false
        };

        var httpClient = _webApplicationFactory?.CreateClient();

        if (httpClient == null)
        {
            throw new InvalidOperationException();
        }

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "TestScheme");

        //httpClient.DefaultRequestHeaders.Add(HeaderNames.AcceptLanguage, "pt-BR");

        return httpClient;
    }

    public void Dispose()
    {
        //nop
    }

    public static void StartApp()
    {
        if (_webApplicationFactory == null)
        {
            Console.WriteLine("Starting Web Application...");

            _webApplicationFactory = new TestAppFactory();

            _webApplicationFactory.CreateDefaultClient();
        }
    }

    //public static PomodoriumContext GetDbContext()
    //{
    //    var db = _webApplicationFactory?.GetDbContext();

    //    if (db == null)
    //    {
    //        throw new InvalidOperationException();
    //    }

    //    return db;
    //}

    public static void StopApp()
    {
        if (_webApplicationFactory != null)
        {
            Console.WriteLine("Shutting down Web Application...");

            _webApplicationFactory.Dispose();

            _webApplicationFactory = null;
        }
    }
}