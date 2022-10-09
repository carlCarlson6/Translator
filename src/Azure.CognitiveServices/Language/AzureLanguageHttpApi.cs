using Azure.CognitiveServices.Language.Models;

namespace Azure.CognitiveServices.Language;

// TODO - implement
// TODO - setup http client
// TODO - inject service
public class AzureLanguageHttpApi : IAzureLanguageApi
{
    public Task<IEnumerable<LanguageDetectionResponse>> DetectLanguage(IEnumerable<DetectLanguageRequest> request)
    {
        throw new NotImplementedException();
    }

    public Task<TranslationResponse> Translate(TranslationRequest request)
    {
        throw new NotImplementedException();
    }
}