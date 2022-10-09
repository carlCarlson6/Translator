using Azure;
using Azure.Data.Tables;
using TranslatorWebApp.Common.Core;
using static System.String;

namespace TranslatorWebApp.Common.Infrastructure.AzureStorageTables;

public class TranslationDocumentTableEntity : ITableEntity
{
    public string PartitionKey { get; set; } = "Translation";
    public string RowKey { get; set; } = Empty;

    public Status Status { get; set; }
    public string OriginalText { get; set; } = Empty;
    public string TranslatedText { get; set; } = Empty;
    
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public static TranslationDocumentTableEntity From(TranslationDocument doc) => new()
    {
        RowKey = doc.Id.ToString(),
        Status = doc.Status,
        OriginalText = doc.OriginalText.ToString(),
        TranslatedText = doc.Translation is null ? Empty : doc.Translation.ToString()
    };

    public TranslationDocument ToDomain() => new(
        Guid.Parse(RowKey),
        new TranslationText(OriginalText),
        IsNullOrWhiteSpace(TranslatedText)
            ? null
            : new TranslationText(TranslatedText),
        Status);
}