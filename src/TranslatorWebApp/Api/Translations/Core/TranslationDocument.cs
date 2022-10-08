using Contracts.Events;

namespace TranslatorWebApp.Api.Translations.Core;

public record TranslationDocument(Guid Id, TranslationText OriginalText, TranslationText? Translation, Status Status)
{
    public static (TranslationDocument doc, TranslationDocumentCreated @event) Create(Guid id, string originalText)
    {
        var doc = new TranslationDocument(id, new TranslationText(originalText), null, Status.Pending);
        return (doc, new TranslationDocumentCreated(doc.Id, doc.OriginalText.ToString()));
    }
}