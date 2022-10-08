using Azure;
using Azure.Data.Tables;
using TranslatorWebApp.Api.Translations.Core;

namespace TranslatorWebApp.Api.Translations.Infrastructure;

public class TranslationDocumentTableEntity : ITableEntity
{
    public string PartitionKey { get; set; } = "Translation";
    public string RowKey { get; set; } = String.Empty;

    public Status Status { get; set; }
    public string OriginalText { get; set; } = String.Empty;
    public string TranslatedText { get; set; } = String.Empty;
    
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public static TranslationDocumentTableEntity From(TranslationDocument doc) => new()
    {
        RowKey = doc.Id.ToString(),
        Status = doc.Status,
        OriginalText = doc.OriginalText.ToString(),
        TranslatedText = doc.Translation is null ? String.Empty : doc.Translation.ToString()
    };

    public TranslationDocument ToDomain() => new(
        Guid.Parse(RowKey),
        new TranslationText(OriginalText),
        String.IsNullOrWhiteSpace(TranslatedText)
            ? null
            : new TranslationText(TranslatedText),
        Status);
}