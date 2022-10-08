using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using TranslatorWebApp.Api.Common;
using TranslatorWebApp.Api.Infrastructure;
using TranslatorWebApp.Api.Translations.Core;

namespace TranslatorWebApp.Api.Translations;

// ReSharper disable all UnusedMember.Global

[ApiController]
[Route(ApiRoutes.Translations)]
public class TranslationsController
{
    // TODO - add logging
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

public record PostTranslationRequest(string Text);
public record PostTranslationResponse(Guid TranslationId);

public record GetTranslationResponse(TranslationDocumentDto TranslationDocument)
{
    public static GetTranslationResponse From(TranslationDocument doc) => new(TranslationDocumentDto.From(doc));
}
public record TranslationDocumentDto(Guid Id, string OriginalText, string TranslationStatus, string? TranslatedText)
{
    public static TranslationDocumentDto From(TranslationDocument doc) => new TranslationDocumentDto(
        doc.Id,
        doc.OriginalText.ToString(),
        doc.Status.GetDisplayName(),
        doc.Translation is null ? null : doc.Translation.ToString());
}