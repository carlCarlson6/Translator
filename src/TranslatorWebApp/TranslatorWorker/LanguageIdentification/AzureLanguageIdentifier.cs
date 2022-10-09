using Azure.CognitiveServices.Language;
using TranslatorWebApp.Common.Core;
using TranslatorWebApp.TranslatorWorker.LanguageIdentification.Core;

namespace TranslatorWebApp.TranslatorWorker.LanguageIdentification;

public class AzureLanguageIdentifier : ILanguageIdentifier
{
    private readonly IAzureLanguageApi _languageApi;
    public AzureLanguageIdentifier(IAzureLanguageApi languageApi) => _languageApi = languageApi;

    public async Task<ILanguageIdentificationResult> Execute(TranslationText text)
    {
        var languages = (await _languageApi.DetectLanguage(text.ToString())).ToList();
        if (!languages.Any())
            return new LanguageIdentificationKoResult();

        var languageCandidate = languages
            .Where(l => l.IsTranslationSupported).MaxBy(l => l.Score);
        if (languageCandidate is null)
            return new LanguageIdentificationKoResult();

        return new LanguageIdentificationOkResult(languageCandidate.Language);
    }
}