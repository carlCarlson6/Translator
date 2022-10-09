using Azure.CognitiveServices.Language.Models;

namespace Azure.CognitiveServices;

public interface IAzureLanguageApi
{
    Task<IEnumerable<LanguageDetectionResponse>> DetectLanguage(IEnumerable<DetectLanguageRequest> request);
    Task<TranslationResponse> Translate(TranslationRequest request);
}