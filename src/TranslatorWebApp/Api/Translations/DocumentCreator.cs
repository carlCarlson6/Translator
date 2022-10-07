using Rebus.Bus;
using TranslatorWebApp.Api.Translations.Core;

namespace TranslatorWebApp.Api.Translations;

public class DocumentCreator
{
    private readonly ITranslationDocumentsRepository _repository;
    private readonly IBus _bus;

    public DocumentCreator(ITranslationDocumentsRepository repository, IBus bus) =>
        (_repository, _bus) = (repository, bus);

    // TODO - add logging
    public async Task<TranslationDocument> Execute(string text)
    {
        var (doc, @event) = TranslationDocument.Create(text);
        await _repository.Upsert(doc);
        await _bus.Send(@event);
        return doc;
    }
}