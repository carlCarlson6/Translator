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
        var languageIdentificationResult = await _azureLanguageIdentifier.Execute(new TranslationText(message.Text));
        await (languageIdentificationResult switch
        {
            LanguageIdentificationOkResult ok => HandleLanguageIdentificationOkResult(message.DocumentId, message.Text, ok.LanguageCode),
            LanguageIdentificationKoResult    => HandleLanguageIdentificationKoResult(message.DocumentId),
            _                                 => throw new ArgumentOutOfRangeException(nameof(languageIdentificationResult))
        });
    }
    
    private async Task UpdateTranslationDocumentStatus(Guid translationId)
    {
        var doc = await _repository.Find(translationId);
        if (doc is null)
            throw new TranslationDocumentNotFound(translationId);
        await _repository.Upsert(doc with { Status = Status.InProcess });
    }

    private Task HandleLanguageIdentificationOkResult(Guid documentId, string text, string languageCode) =>
        languageCode.Equals("es", StringComparison.InvariantCultureIgnoreCase) switch
        {
            true  => _bus.Send(new DocumentTranslated(documentId, text)),
            false => _bus.Send(new DocumentLanguageDetected(documentId, text, languageCode))
        };
    
    private async Task HandleLanguageIdentificationKoResult(Guid documentId) => 
        await _bus.Send(new LanguageCouldNotBeDetected(documentId));
}