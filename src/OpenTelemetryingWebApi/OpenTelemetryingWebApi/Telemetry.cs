using System.Diagnostics;

namespace OpenTelemetryingWebApi;

public static class Telemetry
{
    public const string ActivitySourceName = "AnotherApp.Api";

    public static readonly ActivitySource ActivitySource = new(ActivitySourceName);
}
