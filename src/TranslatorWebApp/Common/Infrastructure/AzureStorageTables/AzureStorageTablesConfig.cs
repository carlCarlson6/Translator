namespace TranslatorWebApp.Common.Infrastructure.AzureStorageTables;

public class AzureStorageTablesConfig
{
    public string ConnectionString { get; set; } = string.Empty;
    public string TranslationDocumentsTable { get; set; } = "TRANSLATIONDOCUMENTS";
}