using Microsoft.OpenApi.Extensions;
using TranslatorWebApp.Common.Core;

namespace TranslatorWebApp.Api.Translations.Messages;

public record TranslationDocumentDto(Guid Id, string OriginalText, string TranslationStatus, string? TranslatedText)
{
    public static TranslationDocumentDto From(TranslationDocument doc) => new TranslationDocumentDto(
        doc.Id,
        doc.OriginalText.ToString(),
        doc.Status.GetDisplayName(),
        doc.Translation?.ToString());
}