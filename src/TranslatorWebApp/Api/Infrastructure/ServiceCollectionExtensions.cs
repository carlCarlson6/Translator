using TranslatorWebApp.Api.Translations;

namespace TranslatorWebApp.Api.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiTranslationServices(this IServiceCollection services) => services
        .AddTransient<DocumentCreator>();
}