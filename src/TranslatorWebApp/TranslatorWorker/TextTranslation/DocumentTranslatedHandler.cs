using Contracts.Events;
using Rebus.Handlers;
using TranslatorWebApp.Common.Core;
using TranslatorWebApp.Common.Core.Errors;

namespace TranslatorWebApp.TranslatorWorker.TextTranslation;

public class DocumentTranslatedHandler : IHandleMessages<DocumentTranslated>
{
    private readonly ITranslationDocumentsRepository _repository;
    public DocumentTranslatedHandler(ITranslationDocumentsRepository repository) => _repository = repository;

    public async Task Handle(DocumentTranslated message)
    {
        var doc = await _repository.Find(message.DocumentId);
        if (doc is null)
            throw new TranslationDocumentNotFound(message.DocumentId);
        await _repository.Upsert(doc with
        {
            Status = Status.Completed, 
            Translation = new TranslationText(message.Translation)
        });
    }
}