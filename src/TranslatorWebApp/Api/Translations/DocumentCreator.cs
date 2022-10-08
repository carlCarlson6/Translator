using Rebus.Bus;
using TranslatorWebApp.Api.Translations.Core;
using TranslatorWebApp.Shared;

namespace TranslatorWebApp.Api.Translations;

public class DocumentCreator
{
    private readonly ITranslationDocumentsRepository _repository;
    private readonly IBus _bus;
    private readonly IGuidGenerator _guidGenerator;

    public DocumentCreator(ITranslationDocumentsRepository repository, IBus bus, IGuidGenerator guidGenerator)
    {
        _repository = repository;
        _bus = bus;
        _guidGenerator = guidGenerator;
    }

    // TODO - add logging
    public async Task<TranslationDocument> Execute(string text)
    {
        var (doc, @event) = TranslationDocument.Create(_guidGenerator.New(), text);
        await _repository.Upsert(doc);
        await _bus.Send(@event);
        return doc;
    }
}