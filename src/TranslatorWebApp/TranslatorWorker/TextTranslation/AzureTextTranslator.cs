using Azure.CognitiveServices;
using Azure.CognitiveServices.Language.Models;
using TranslatorWebApp.Common.Core;
using TranslatorWebApp.TranslatorWorker.TextTranslation.Core;

namespace TranslatorWebApp.TranslatorWorker.TextTranslation;

public class AzureTextTranslator : ITextTranslator
{
    private readonly IAzureLanguageApi _api;
    public AzureTextTranslator(IAzureLanguageApi api) => _api = api;

    public async Task<ITextTranslationResult> Execute(TranslationText text, string sourceLanguage)
    {
        var request = new TranslationRequest(
            sourceLanguage, 
            "es", 
            new List<TranslationRequestBodyText> { new(text.ToString()) });

        var response = (await _api.Translate(request)).ToList();
        if (!response.Any())
            return new TextTranslationKoResult();

        var translatedText = response.FirstOrDefault()?.Translations.FirstOrDefault()?.Text;
        if (string.IsNullOrWhiteSpace(translatedText)) 
            return new TextTranslationKoResult();

        return new TextTranslationOkResult(translatedText);
    }
}