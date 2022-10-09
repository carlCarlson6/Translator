using TranslatorWebApp.Common.Core;
using TranslatorWebApp.Common.Infrastructure.AzureStorageTables;

namespace TranslatorWebApp.Common.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services) => services
        .AddSingleton<IGuidGenerator, CsharpGuidGenerator>()
        .AddSingleton<ITranslationDocumentsRepository, AzureStorageTableTranslationDocumentsRepository>();
}