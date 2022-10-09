using Azure;
using Azure.Data.Tables;
using TranslatorWebApp.Common.Core;

namespace TranslatorWebApp.Common.Infrastructure.AzureStorageTables;

public class AzureStorageTableTranslationDocumentsRepository : ITranslationDocumentsRepository
{
    private readonly TableClient _tableClient;
    
    public AzureStorageTableTranslationDocumentsRepository(TableClient tableClient) => _tableClient = tableClient;

    public async Task<TranslationDocument?> Find(Guid translationId)
    {
        try
        {
            var response = await _tableClient.GetEntityAsync<TranslationDocumentTableEntity>("Translation", translationId.ToString());
            return response?.Value.ToDomain();
        }
        catch (RequestFailedException)
        {
            return null;
        }
    }

    public async Task Upsert(TranslationDocument doc)
    {
        var response = await _tableClient.UpsertEntityAsync(TranslationDocumentTableEntity.From(doc));
        if (response.IsError)
            throw new Exception(response.ReasonPhrase);
    }
}