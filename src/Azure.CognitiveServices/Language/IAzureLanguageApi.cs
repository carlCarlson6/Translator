namespace Azure.CognitiveServices.Language;

// TODO - implement
public interface IAzureLanguageApi
{
    Task<IEnumerable<LanguageDetectionResponse>> DetectLanguage(string text);
    Task Translate(string text, string destinationLang);
}