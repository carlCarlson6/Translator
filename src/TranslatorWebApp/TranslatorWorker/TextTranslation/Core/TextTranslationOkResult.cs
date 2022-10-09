namespace TranslatorWebApp.TranslatorWorker.TextTranslation.Core;

public record TextTranslationOkResult(string TranslatedText) : ITextTranslationResult;