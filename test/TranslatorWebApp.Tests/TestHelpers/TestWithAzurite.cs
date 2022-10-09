using Azure.Data.Tables;
using DotNet.Testcontainers.Builders;
using Rebus.TestHelpers;
using TranslatorWebApp.Common.Core;
using TranslatorWebApp.Common.Infrastructure.AzureStorageTables;
using TranslatorWebApp.Tests.TestHelpers.Azurite;
using Xunit;

namespace TranslatorWebApp.Tests.TestHelpers;

public class TestWithAzurite : IAsyncLifetime
{
    protected readonly FakeBus FakeBus = new();
    private readonly AzuriteTestContainer _container;
    protected readonly IGuidGenerator FakeGuidGenerator = new FakeGuidGenerator();

    protected TestWithAzurite()
    {
        _container = new TestcontainersBuilder<AzuriteTestContainer>()
            .WithAzurite(new AzuriteTestContainerConfig())
            .Build();
    }
    
    protected TableClient GivenTableClient() => new TableServiceClient(_container.ConnectionString)
        .GetTableClient(new AzureStorageTablesConfig().TranslationDocumentsTable);
    
    protected async Task<TableClient> InitTableClientAsync()
    {
        var tableClient = new TableServiceClient(_container.ConnectionString)
            .GetTableClient(new AzureStorageTablesConfig().TranslationDocumentsTable);
        await tableClient.CreateIfNotExistsAsync();
        return tableClient;
    }

    public Task InitializeAsync() => _container.StartAsync();
    public Task DisposeAsync() => _container.DisposeAsync().AsTask();
}