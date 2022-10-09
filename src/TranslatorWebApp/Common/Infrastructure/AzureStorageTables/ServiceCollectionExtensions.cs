using Azure.Data.Tables;

namespace TranslatorWebApp.Common.Infrastructure.AzureStorageTables;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAzureStorageTableClient(
        this IServiceCollection services, 
        string tableName, 
        string connectionString
    ) => services
        .AddSingleton(new TableServiceClient(connectionString).GetTableClient(tableName))
        .AddHostedService<AzureStorageTableInitializer>();
}