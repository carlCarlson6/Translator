using TranslatorWebApp.Api.Translations.Core;

namespace TranslatorWebApp.Api.Translations.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiTranslationServices(this IServiceCollection services) => services
        .AddTransient<DocumentCreator>()
        .AddSingleton<ITranslationDocumentsRepository, AzureStorageTableTranslationDocumentsRepository>();
}