using Microsoft.AspNetCore.Mvc;
using TranslatorWebApp.Api.Infrastructure;

namespace TranslatorWebApp.Api.Health;

[ApiController]
[Route(ApiRoutes.Health)]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult SayHello() => new ObjectResult(new HealthControllerResponse("hello world :) i'm alive"));
    
    [HttpGet("{name}")]
    public IActionResult SayHello(string name) => new ObjectResult(new HealthControllerResponse($"hello {name} :) i'm alive"));
}

public record HealthControllerResponse(string Message);