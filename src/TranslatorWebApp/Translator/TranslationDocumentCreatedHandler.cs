using Azure.Data.Tables;
using Contracts.Events;
using Rebus.Handlers;

namespace TranslatorWebApp.Translator;

public class TranslationDocumentCreatedHandler : IHandleMessages<TranslationDocumentCreated>
{
    private readonly TableClient _tableClient;

    public TranslationDocumentCreatedHandler(TableClient tableClient) => _tableClient = tableClient;

    public async Task Handle(TranslationDocumentCreated message)
    {
        await UpdateTranslationDocumentStatus(message.DocumentId);
    }

    private Task UpdateTranslationDocumentStatus(Guid translationId)
    {
        
        
        throw new NotImplementedException();
    }
}