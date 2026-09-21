using DailymotionSDK.Configuration;
using DailymotionSDK.Demo.Services;
using DailymotionSDK.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DailymotionSDK.Demo;

/// <summary>
/// DailyMotion SDK Demo Application
/// Tests all SDK functionality in a real environment
/// </summary>
public class Program
{
    public static async Task Main(string[] args)
    {
        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        try
        {
            Console.WriteLine("=== DailyMotion SDK Demo Application ===");
            Console.WriteLine("This demo will test all SDK functionality in a real environment.");
            Console.WriteLine("Make sure you have configured your credentials in appsettings.json or user secrets.\n");

            var host = CreateHostBuilder(args).Build();

            // Run the demo
            var demoService = host.Services.GetRequiredService<IDemoService>();
            //await demoService.RunDemoAsync();

            Console.WriteLine("\n=== Client Credentials with Private Keys Flow ===");

            await demoService.RunDemoAsync();

            Console.WriteLine("\n=== Demo completed successfully! ===");
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Demo application terminated unexpectedly");
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine("Please check your configuration and try again.");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureAppConfiguration((context, config) =>
            {
                // Clear existing sources and rebuild with proper priority
                config.Sources.Clear();

                // Add configuration sources in order of priority (last wins)
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                config.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);

                // Add user secrets with highest priority (always in development)
                config.AddUserSecrets<Program>(optional: false);

                // Add environment variables
                config.AddEnvironmentVariables();

                // Add command line arguments
                if (args != null)
                {
                    config.AddCommandLine(args);
                }
            })
            .ConfigureServices((context, services) =>
            {
                // 1. Register the Dailymotion SDK
                services.AddDailymotionSDK(context.Configuration.GetSection("DailymotionOptions"));

                // 2. Bind and register DemoOptions as a raw singleton
                var demoOptions = new DemoOptions();
                context.Configuration.GetSection("DemoOptions").Bind(demoOptions);
                services.AddSingleton(demoOptions);

                // 3. Register the DemoService
                services.AddScoped<IDemoService, DemoService>();
            });
}
