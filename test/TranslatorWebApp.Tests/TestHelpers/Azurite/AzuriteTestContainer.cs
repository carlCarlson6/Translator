using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.Logging;

namespace TranslatorWebApp.Tests.TestHelpers.Azurite;

public class AzuriteTestContainer : TestcontainersContainer
{
    public int BlobPort => GetMappedPublicPort(BlobContainerPort);
    public int QueuePort => GetMappedPublicPort(QueueContainerPort);
    public int TablePort => GetMappedPublicPort(TableContainerPort);
    public int BlobContainerPort { get; set; }
    public int QueueContainerPort { get; set; }
    public int TableContainerPort { get; set; }
    
    public string ConnectionString => BuildConnectionString();
    
    protected AzuriteTestContainer(ITestcontainersConfiguration configuration, ILogger logger) : 
        base(configuration, logger) { }
    
    private string BuildConnectionString()
    {
        const string accountName = "devstoreaccount1";
        const string accountKey = "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==";
        var endpointBuilder = new UriBuilder("http", this.Hostname, -1, accountName);
        IDictionary<string, string> connectionString = new Dictionary<string, string>();
        connectionString.Add("DefaultEndpointsProtocol", endpointBuilder.Scheme);
        connectionString.Add("AccountName", accountName);
        connectionString.Add("AccountKey", accountKey);
        
        if (BlobContainerPort > 0)
        {
            endpointBuilder.Port = BlobPort;
            connectionString.Add("BlobEndpoint", endpointBuilder.ToString());
        }

        if (QueueContainerPort > 0)
        {
            endpointBuilder.Port = QueuePort;
            connectionString.Add("QueueEndpoint", endpointBuilder.ToString());
        }

        if (TableContainerPort > 0)
        {
            endpointBuilder.Port = TablePort;
            connectionString.Add("TableEndpoint", endpointBuilder.ToString());
        }

        return string.Join(";", connectionString.Select(kvp => $"{kvp.Key}={kvp.Value}"));
    }
}