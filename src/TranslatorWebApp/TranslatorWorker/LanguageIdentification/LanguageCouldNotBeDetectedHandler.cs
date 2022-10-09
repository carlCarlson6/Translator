using Contracts.Events;
using Rebus.Handlers;
using TranslatorWebApp.Common.Core;
using TranslatorWebApp.Common.Core.Errors;

namespace TranslatorWebApp.TranslatorWorker.LanguageIdentification;

public class LanguageCouldNotBeDetectedHandler : IHandleMessages<LanguageCouldNotBeDetected>
{
    private readonly ITranslationDocumentsRepository _repository;
    
    public LanguageCouldNotBeDetectedHandler(ITranslationDocumentsRepository repository) => _repository = repository;

    public async Task Handle(LanguageCouldNotBeDetected message)
    {        
        var doc = await _repository.Find(message.DocumentId);
        if (doc is null)
            throw new TranslationDocumentNotFound(message.DocumentId);
        await _repository.Upsert(doc with
        {
            Status = Status.Completed, 
            Translation = new TranslationText("CLOUD_NOT_TRANSLATE_ORIGINAL_TEXT")
        });
    }
}