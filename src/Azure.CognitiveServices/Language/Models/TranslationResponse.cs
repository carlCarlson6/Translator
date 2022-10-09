namespace Azure.CognitiveServices.Language.Models;

public record TranslationResponse(IEnumerable<AzureTranslation> Translations);

public record AzureTranslation(string Text, string To);

