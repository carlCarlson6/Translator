using Microsoft.AspNetCore.Mvc;
using TranslatorWebApp.Api.Infrastructure;

namespace TranslatorWebApp.Api.Translations;

[ApiController]
[Route(ApiRoutes.Translations)]
public class TranslationsController
{
    [HttpGet("{translationId:guid}")]
    public IActionResult GetTranslation(Guid translationId) => throw new NotImplementedException();

    [HttpPost]
    public IActionResult PostTranslation([FromBody] PostTranslationRequest request) =>
        throw new NotImplementedException();
}

public record PostTranslationRequest(string Text);