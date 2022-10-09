using TranslatorWebApp.TranslatorWorker.LanguageIdentification;
using TranslatorWebApp.TranslatorWorker.LanguageIdentification.Core;

namespace TranslatorWebApp.TranslatorWorker.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTranslatorWorkerServices(this IServiceCollection services) => services
        .AddTransient<ILanguageIdentifier, AzureLanguageIdentifier>();
}