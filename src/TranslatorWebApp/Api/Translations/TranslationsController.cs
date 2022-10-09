using System.Net;
using Microsoft.AspNetCore.Mvc;
using TranslatorWebApp.Api.Infrastructure;
using TranslatorWebApp.Api.Translations.Messages;
using TranslatorWebApp.Common.Core;
using TranslatorWebApp.Common.Core.Errors;

namespace TranslatorWebApp.Api.Translations;

// ReSharper disable all UnusedMember.Global

[ApiController]
[Route(ApiRoutes.Translations)]
public class TranslationsController
{
    private readonly DocumentCreator _documentCreator;
    private readonly ITranslationDocumentsRepository _repository;

    public TranslationsController(DocumentCreator documentCreator, ITranslationDocumentsRepository repository) =>
        (_documentCreator, _repository) = (documentCreator, repository);

    [HttpGet("{translationId:guid}")]
    public async Task<IActionResult> GetTranslation(Guid translationId)
    {
        var maybeDoc = await _repository.Find(translationId);
        return maybeDoc is null
            ? new NotFoundResult()
            : new OkObjectResult(GetTranslationResponse.From(maybeDoc));
    } 

    [HttpPost]
    public async Task<IActionResult> PostTranslation([FromBody] PostTranslationRequest request)
    {
        try
        {
            var doc = await _documentCreator.Execute(request.Text);
            return new ObjectResult(new PostTranslationResponse(doc.Id)) { StatusCode = (int)HttpStatusCode.Accepted };
        }
        catch (InvalidTranslationText error)
        {
            return new ObjectResult(ApiErrorResponse.FromApiError(error)) { StatusCode = (int)error.Code };
        }
    }
}