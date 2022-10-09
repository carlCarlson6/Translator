namespace Azure.CognitiveServices.Language.Models;

public record LanguageDetectionResponse(
    string Language, 
    double Score, 
    bool IsTranslationSupported,
    bool IsTransliterationSupported);