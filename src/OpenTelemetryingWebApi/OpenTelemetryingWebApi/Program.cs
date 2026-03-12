using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace OpenTelemetryingWebApi;

public class Program
{
    // Custom metrics for the application
    public static Meter greeterMeter = new Meter("OTel.Example", "1.0.0");
    public static Counter<int>? countGreetings;

    // Custom ActivitySource for the application
    public static ActivitySource greeterActivitySource = new ActivitySource("OTel.Example");

    public static void Main(string[] args)
    {
        countGreetings = greeterMeter.CreateCounter<int>("greetings.count", description: "Counts the number of greetings");

        // Standard web API startup code
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        // Setup logging to be exported via OpenTelemetry
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        var otel = builder.Services.AddOpenTelemetry();

        // Add Metrics for ASP.NET Core and our custom metrics and export via OTLP
        otel.WithMetrics(metrics =>
        {
            // Metrics provider from OpenTelemetry
            metrics.AddAspNetCoreInstrumentation();
            //Our custom metrics
            metrics.AddMeter(greeterMeter.Name);
            // Metrics provides by ASP.NET Core in .NET 8
            metrics.AddMeter("Microsoft.AspNetCore.Hosting");
            metrics.AddMeter("Microsoft.AspNetCore.Server.Kestrel");
        });

        // Add Tracing for ASP.NET Core and our custom ActivitySource and export via OTLP
        otel.WithTracing(tracing =>
        {
            tracing.AddAspNetCoreInstrumentation();
            tracing.AddHttpClientInstrumentation();
            tracing.AddSource(greeterActivitySource.Name);
            tracing.AddSource("AnotherApp.Api");
        });

        // Export OpenTelemetry data via OTLP, using env vars for the configuration
        var OtlpEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
        if (OtlpEndpoint != null)
        {
            otel.UseOtlpExporter();
        }

        var app = builder.Build();

        app.MapControllers();

        // FIXME:Old test! Remove later!
        // app.MapGet("/", SendGreeting);

        app.Run();
    }

    async static Task<string> SendGreeting(ILogger<Program> logger)
    {
        // Create a new Activity scoped to the method
        using var activity = greeterActivitySource.StartActivity("GreeterActivity");

        // Log a message
        logger.LogInformation("Sending greeting");

        // Increment the custom counter
        countGreetings!.Add(1);

        // Add a tag to the Activity
        activity?.SetTag("greeting", "Hello World!");

        return "Hello World!";
    }
}
