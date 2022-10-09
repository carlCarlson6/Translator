using TranslatorWebApp.Common.Core;

namespace TranslatorWebApp.TranslatorWorker.TextTranslation.Core;

public interface ITextTranslator
{
    Task<ITextTranslationResult> Execute(TranslationText text, string sourceLanguage);
}