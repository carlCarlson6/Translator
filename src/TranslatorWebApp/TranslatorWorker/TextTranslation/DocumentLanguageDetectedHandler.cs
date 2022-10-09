using Contracts.Events;
using Rebus.Bus;
using Rebus.Handlers;
using TranslatorWebApp.Common.Core;
using TranslatorWebApp.TranslatorWorker.TextTranslation.Core;

namespace TranslatorWebApp.TranslatorWorker.TextTranslation;

public class DocumentLanguageDetectedHandler : IHandleMessages<DocumentLanguageDetected>
{
    private readonly ITextTranslator _translator;
    private readonly IBus _bus;
    
    public DocumentLanguageDetectedHandler(ITextTranslator translator, IBus bus) => (_translator, _bus) = (translator, bus);

    public async Task Handle(DocumentLanguageDetected message)
    {
        var result = await _translator.Execute(new TranslationText(message.Text), message.LanguageCode);
        await (result switch
        {
            TextTranslationOkResult ok => _bus.Send(new DocumentTranslated(message.DocumentId, ok.TranslatedText)),
            TextTranslationKoResult    => throw new ErrorTranslatingDocument(message.DocumentId, message.Text, message.LanguageCode),
            _                          => throw new ArgumentOutOfRangeException(nameof(result))
        });
    }
}