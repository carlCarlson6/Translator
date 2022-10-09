using System.Collections.Immutable;
using Azure.CognitiveServices.Language.Models;

namespace Azure.CognitiveServices.Language;

public class AzureLanguageHttpApi : IAzureLanguageApi
{
    private readonly HttpClient _httpClient;
    public AzureLanguageHttpApi(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<IEnumerable<LanguageDetectionResponse>> DetectLanguage(IEnumerable<DetectLanguageRequest> request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/detect?api-version=3.0", request);
            if (!response.IsSuccessStatusCode)
                return ImmutableList<LanguageDetectionResponse>.Empty;

            return await response.Content.ReadAsAsync<IEnumerable<LanguageDetectionResponse>>();
        }
        catch (Exception)
        {
            return ImmutableList<LanguageDetectionResponse>.Empty;
        }
    }

    public async Task<TranslationResponse> Translate(TranslationRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                $"/translate?api-version=3.0&from={request.From}&to={request.To}", 
                request.Body);
            if (!response.IsSuccessStatusCode)
                return new TranslationResponse(ImmutableList<AzureTranslation>.Empty);

            return await response.Content.ReadAsAsync<TranslationResponse>();
        }
        catch (Exception)
        {
            return new TranslationResponse(ImmutableList<AzureTranslation>.Empty);
        }
    }
}