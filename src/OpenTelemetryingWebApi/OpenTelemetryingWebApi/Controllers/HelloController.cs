using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace OpenTelemetryingWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HelloController : ControllerBase
{
    [HttpGet]
    public async Task<string> Get()
    {
        using var activity = Telemetry.ActivitySource.StartActivity("Psyching myself up to say hello!");

        await Task.Delay(TimeSpan.FromSeconds(3)); // Simulate some work

        activity?.AddEvent(new ActivityEvent("Gonna just say it!"));

        await Task.Delay(TimeSpan.FromSeconds(3)); // Simulate some work

        activity?.AddEvent(new ActivityEvent("Here goes!"));

        return "Hello from controller!";
    }
}
