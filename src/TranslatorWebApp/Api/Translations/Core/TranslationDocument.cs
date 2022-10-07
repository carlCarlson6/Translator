using Contracts.Events;

namespace TranslatorWebApp.Api.Translations.Core;

public record TranslationDocument(Guid Id, TranslationText OriginalText, TranslationText? Translation, Status Status)
{
    public static (TranslationDocument doc, TranslationDocumentCreated @event) Create(string originalText)
    {
        var doc = new TranslationDocument(Guid.NewGuid(), new TranslationText(originalText), null, Status.Pending);
        return (doc, new TranslationDocumentCreated(doc.Id, doc.OriginalText.ToString()));
    }
}

public class TranslationText
{
    private readonly string _value;

    public TranslationText(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new Exception(); // TODO - create proper exception
        _value = value;
    }

    public override string ToString() => _value;
}

public enum Status
{
    Pending,
    InProcess,
    Completed,
}