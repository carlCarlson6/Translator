namespace Azure.CognitiveServices.Language;

public interface IAzureLanguageApi
{
    Task<IEnumerable<LanguageDetectionResponse>> DetectLanguage(string text);
}

public record LanguageDetectionResponse(
    string Language, 
    double Score, 
    bool IsTranslationSupported,
    bool IsTransliterationSupported);