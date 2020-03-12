using System;
using System.Threading.Tasks;
using Conference.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Conference
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .WriteTo.AzureWebAppDiagnostics()
                .Enrich.FromLogContext()
                .CreateLogger();

            try
            {
                var host = CreateHostBuilder(args).Build();

                using var scope = host.Services.CreateScope();

                var context = scope.ServiceProvider.GetRequiredService<ConferenceContext>();

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                await context.Database.EnsureDeletedAsync();

                await context.Database.EnsureCreatedAsync();

                await context.SeedAsync(userManager);

                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(web => web.UseStartup<Startup>())
                .UseSerilog();
        }
    }
}
