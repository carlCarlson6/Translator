using TranslatorWebApp.Common.Core;

namespace TranslatorWebApp.Api.Translations.Messages;

public record GetTranslationResponse(TranslationDocumentDto TranslationDocument)
{
    public static GetTranslationResponse From(TranslationDocument doc) => new(TranslationDocumentDto.From(doc));
}