using TranslatorWebApp.Common.Core;

namespace TranslatorWebApp.TranslatorWorker.LanguageIdentification.Core;

public interface ILanguageIdentifier
{
    Task<ILanguageIdentificationResult> Execute(TranslationText text);
}