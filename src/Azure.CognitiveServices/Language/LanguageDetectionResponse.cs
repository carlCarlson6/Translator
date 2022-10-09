namespace Azure.CognitiveServices.Language;

public record LanguageDetectionResponse(
    string Language, 
    double Score, 
    bool IsTranslationSupported,
    bool IsTransliterationSupported);