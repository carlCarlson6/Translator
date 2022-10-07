using Microsoft.AspNetCore.Mvc;
using TranslatorWebApp.Api.Infrastructure;

namespace TranslatorWebApp.Api.Translations;

[ApiController]
[Route(ApiRoutes.Translations)]
public class TranslationsController
{
    private readonly DocumentCreator _documentCreator;

    public TranslationsController(DocumentCreator documentCreator) => _documentCreator = documentCreator;

    [HttpGet("{translationId:guid}")]
    public Task<IActionResult> GetTranslation(Guid translationId) => throw new NotImplementedException();

    [HttpPost]
    public async Task<IActionResult> PostTranslation([FromBody] PostTranslationRequest request)
    {
        var doc = await _documentCreator.Execute(request.Text);
        return new OkObjectResult(new PostTranslationResponse(doc.Id));
    }
}

public record PostTranslationRequest(string Text);
public record PostTranslationResponse(Guid TranslationId);

public record GetTranslationResponse();