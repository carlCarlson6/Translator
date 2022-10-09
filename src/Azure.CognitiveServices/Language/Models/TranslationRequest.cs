namespace Azure.CognitiveServices.Language.Models;

public record TranslationRequest(string From, string To, IEnumerable<TranslationRequestBodyText> Body);

public record TranslationRequestBodyText(string Text);