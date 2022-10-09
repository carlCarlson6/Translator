using Microsoft.Azure.Storage;
using TranslatorWebApp.Common.Core;
using TranslatorWebApp.Common.Infrastructure.AzureStorageTables;

namespace TranslatorWebApp.Common.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services) => services
        .AddSingleton<IGuidGenerator, CsharpGuidGenerator>()
        .AddSingleton<ITranslationDocumentsRepository, AzureStorageTableTranslationDocumentsRepository>();
    
    public static CloudStorageAccount GetStorageAccount(this IConfiguration configuration)
    {
        var connectionString = configuration.GetCloudStorageConnectionString();
        return CloudStorageAccount.Parse(connectionString);
    }

    public static string GetCloudStorageConnectionString(this IConfiguration configuration)
    {
        var connectionString = configuration["StorageAccountConfiguration:ConnectionString"];
        CloudStorageAccount.Parse(connectionString);
        return connectionString;
    }
}