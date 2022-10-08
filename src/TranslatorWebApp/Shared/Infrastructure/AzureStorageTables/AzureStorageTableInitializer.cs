using Azure.Data.Tables;

namespace TranslatorWebApp.Shared.Infrastructure.AzureStorageTables;

public class AzureStorageTableInitializer : BackgroundService
{
    private readonly TableClient _tableClient;

    public AzureStorageTableInitializer(TableClient tableClient) => _tableClient = tableClient;

    // TODO - add logging
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