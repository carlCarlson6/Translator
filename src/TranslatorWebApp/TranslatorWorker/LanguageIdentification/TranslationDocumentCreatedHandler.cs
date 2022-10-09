using Contracts.Events;
using Rebus.Bus;
using Rebus.Handlers;
using TranslatorWebApp.Common.Core;
using TranslatorWebApp.Common.Core.Errors;
using TranslatorWebApp.TranslatorWorker.LanguageIdentification.Core;

namespace TranslatorWebApp.TranslatorWorker.LanguageIdentification;

public class TranslationDocumentCreatedHandler : IHandleMessages<TranslationDocumentCreated>
{
    private readonly ITranslationDocumentsRepository _repository;
    private readonly IBus _bus;
    private readonly ILanguageIdentifier _azureLanguageIdentifier;
    
    public TranslationDocumentCreatedHandler(
        ITranslationDocumentsRepository repository, 
        IBus bus, 
        ILanguageIdentifier azureLanguageIdentifier)
    {
        _repository = repository;
        _bus = bus;
        _azureLanguageIdentifier = azureLanguageIdentifier;
    }

    // TODO - add logging
    public async Task Handle(TranslationDocumentCreated message)
    {
        await UpdateTranslationDocumentStatus(message.DocumentId);
        await IdentifyLanguage(message.DocumentId, new TranslationText(message.Text));
    }
    
    private async Task UpdateTranslationDocumentStatus(Guid translationId)
    {
        var doc = await _repository.Find(translationId);
        if (doc is null)
            throw new TranslationDocumentNotFound(translationId);
        await _repository.Upsert(doc with { Status = Status.InProcess });
    }

    private async Task IdentifyLanguage(Guid docId, TranslationText text)
    {
        var languageIdentificationResult = await _azureLanguageIdentifier.Execute(text);
        await (languageIdentificationResult switch
        {
            LanguageIdentificationOkResult ok => HandleLanguageIdentificationOkResult(docId, text.ToString(), ok.LanguageCode),
            LanguageIdentificationKoResult    => _bus.Send(new LanguageCouldNotBeDetected(docId)),
            _                                 => throw new ArgumentOutOfRangeException(nameof(languageIdentificationResult))
        });
    }

    private Task HandleLanguageIdentificationOkResult(Guid documentId, string text, string languageCode) =>
        languageCode.Equals("es", StringComparison.InvariantCultureIgnoreCase) switch
        {
            true  => _bus.Send(new DocumentTranslated(documentId, text)),
            false => _bus.Send(new DocumentLanguageDetected(documentId, text, languageCode))
        };
}