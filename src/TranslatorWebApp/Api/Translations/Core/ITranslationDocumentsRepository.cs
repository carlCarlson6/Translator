namespace TranslatorWebApp.Api.Translations.Core;

public interface ITranslationDocumentsRepository
{
    Task<TranslationDocument?> Find(Guid translationId);
    Task Upsert(TranslationDocument doc);
}