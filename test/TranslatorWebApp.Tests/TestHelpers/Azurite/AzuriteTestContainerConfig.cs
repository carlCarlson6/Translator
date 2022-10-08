using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;

namespace TranslatorWebApp.Tests.TestHelpers.Azurite;

public class AzuriteTestContainerConfig
{
    public const string DefaultWorkspaceDirectoryPath = "/data/";

    private const string AzuriteImage = "mcr.microsoft.com/azure-storage/azurite:3.18.0";

    private const int DefaultBlobPort = 10000;

    private const int DefaultQueuePort = 10001;

    private const int DefaultTablePort = 10002;

    private AzuriteServices enabledServices = AzuriteServices.All;

    public AzuriteTestContainerConfig() : this(AzuriteImage) { }
    public AzuriteTestContainerConfig(string image) => Image = image;

    [Flags]
    internal enum AzuriteServices
    {
        Blob = 1,
        Queue = 2,
        Table = 4,
        All = Blob | Queue | Table,
    }

    public string Image { get; }

    public IWaitForContainerOS WaitStrategy
    {
        get
        {
            var waitStrategy = Wait.ForUnixContainer();
            waitStrategy = enabledServices.HasFlag(AzuriteServices.Blob) ? waitStrategy.UntilPortIsAvailable(BlobContainerPort) : waitStrategy;
            waitStrategy = enabledServices.HasFlag(AzuriteServices.Queue) ? waitStrategy.UntilPortIsAvailable(QueueContainerPort) : waitStrategy;
            waitStrategy = enabledServices.HasFlag(AzuriteServices.Table) ? waitStrategy.UntilPortIsAvailable(TableContainerPort) : waitStrategy;
            return waitStrategy;
        }
    }

    public int BlobPort { get; set; }

    public int BlobContainerPort { get; set; } = DefaultBlobPort;

    public bool BlobServiceOnlyEnabled
    {
        get => AzuriteServices.Blob.Equals(enabledServices);
        set => enabledServices = value ? AzuriteServices.Blob : AzuriteServices.All;
    }

    public int QueuePort { get; set; }

    public int QueueContainerPort { get; set; } = DefaultQueuePort;

    public bool QueueServiceOnlyEnabled
    {
        get => AzuriteServices.Queue.Equals(enabledServices);
        set => enabledServices = value ? AzuriteServices.Queue : AzuriteServices.All;
    }

    public int TablePort { get; set; }

    public int TableContainerPort { get; set; } = DefaultTablePort;

    public bool TableServiceOnlyEnabled
    {
        get => AzuriteServices.Table.Equals(enabledServices);
        set => enabledServices = value ? AzuriteServices.Table : AzuriteServices.All;
    }

    public bool AllServicesEnabled => AzuriteServices.All.Equals(enabledServices);
    public string Location { get; set; }
    public bool DebugModeEnabled { get; set; }
    public bool SilentModeEnabled { get; set; }
    public bool LooseModeEnabled { get; set; }
    public bool SkipApiVersionCheckEnabled { get; set; }
    public bool ProductStyleUrlDisabled { get; set; }
}