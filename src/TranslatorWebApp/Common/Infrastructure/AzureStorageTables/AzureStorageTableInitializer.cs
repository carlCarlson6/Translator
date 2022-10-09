using Azure.Data.Tables;

namespace TranslatorWebApp.Common.Infrastructure.AzureStorageTables;

public class AzureStorageTableInitializer : BackgroundService
{
    private readonly TableClient _tableClient;

    public AzureStorageTableInitializer(TableClient tableClient) => _tableClient = tableClient;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await _tableClient.CreateIfNotExistsAsync(stoppingToken);
        }
        catch (Exception)
        {
            // ignored
        }
    }
}