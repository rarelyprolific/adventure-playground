using System.Diagnostics;

namespace OpenTelemetryingWebApi;

public static class Telemetry
{
    public static readonly ActivitySource ActivitySource = new("AnotherApp.Api");
}
