using System.Globalization;
using DotNet.Testcontainers.Builders;

namespace TranslatorWebApp.Tests.TestHelpers.Azurite;

public static class TestContainersBuilderAzuriteExtension
{
    public static ITestcontainersBuilder<AzuriteTestContainer> WithAzurite(this ITestcontainersBuilder<AzuriteTestContainer> builder, AzuriteTestContainerConfig configuration)
    {
        var blobServiceEnabled = configuration.AllServicesEnabled || configuration.BlobServiceOnlyEnabled;
        var queueServiceEnabled = configuration.AllServicesEnabled || configuration.QueueServiceOnlyEnabled;
        var tableServiceEnabled = configuration.AllServicesEnabled || configuration.TableServiceOnlyEnabled;

        builder = builder
            .WithImage(configuration.Image)
            .WithWaitStrategy(configuration.WaitStrategy)
            .ConfigureContainer(container =>
            {
                container.BlobContainerPort = blobServiceEnabled ? configuration.BlobContainerPort : 0;
                container.QueueContainerPort = queueServiceEnabled ? configuration.QueueContainerPort : 0;
                container.TableContainerPort = tableServiceEnabled ? configuration.TableContainerPort : 0;
            });

        if (blobServiceEnabled)
        {
            builder = builder
                .WithExposedPort(configuration.BlobContainerPort)
                .WithPortBinding(configuration.BlobPort, configuration.BlobContainerPort);
        }

        if (queueServiceEnabled)
        {
            builder = builder
                .WithExposedPort(configuration.QueueContainerPort)
                .WithPortBinding(configuration.QueuePort, configuration.QueueContainerPort);
        }

        if (tableServiceEnabled)
        {
            builder = builder
                .WithExposedPort(configuration.TableContainerPort)
                .WithPortBinding(configuration.TablePort, configuration.TableContainerPort);
        }

        if (configuration.Location != null)
        {
            builder = builder
                .WithBindMount(configuration.Location, AzuriteTestContainerConfig.DefaultWorkspaceDirectoryPath);
        }

        return builder
        .WithCommand(GetExecutable(configuration))
        .WithCommand(GetEnabledServices(configuration))
        .WithCommand(GetWorkspaceDirectoryPath())
        .WithCommand(GetDebugModeEnabled(configuration))
        .WithCommand(GetSilentModeEnabled(configuration))
        .WithCommand(GetLooseModeEnabled(configuration))
        .WithCommand(GetSkipApiVersionCheckEnabled(configuration))
        .WithCommand(GetProductStyleUrlDisabled(configuration));
    }

    private static string GetExecutable(AzuriteTestContainerConfig configuration)
    {
        if (configuration.BlobServiceOnlyEnabled)
        { 
            return "azurite-blob";
        }

        if (configuration.QueueServiceOnlyEnabled)
        { 
            return "azurite-queue";
        }

        if (configuration.TableServiceOnlyEnabled)
        { 
            return "azurite-table";
        }

        return "azurite";
    }

    private static string[] GetEnabledServices(AzuriteTestContainerConfig configuration)
    {
        const string defaultRemoteEndpoint = "0.0.0.0";

        IList<string> args = new List<string>();

        if (configuration.AllServicesEnabled || configuration.BlobServiceOnlyEnabled)
        {
            args.Add("--blobHost");
            args.Add(defaultRemoteEndpoint);
            args.Add("--blobPort");
            args.Add(configuration.BlobContainerPort.ToString(CultureInfo.InvariantCulture));
        }

        if (configuration.AllServicesEnabled || configuration.QueueServiceOnlyEnabled)
        {
            args.Add("--queueHost");
            args.Add(defaultRemoteEndpoint);
            args.Add("--queuePort");
            args.Add(configuration.QueueContainerPort.ToString(CultureInfo.InvariantCulture));
        }

        if (configuration.AllServicesEnabled || configuration.TableServiceOnlyEnabled)
        {
            args.Add("--tableHost");
            args.Add(defaultRemoteEndpoint);
            args.Add("--tablePort");
            args.Add(configuration.TableContainerPort.ToString(CultureInfo.InvariantCulture));
        }

        return args.ToArray();
    }

    private static string[] GetWorkspaceDirectoryPath() => new[] { "--location", AzuriteTestContainerConfig.DefaultWorkspaceDirectoryPath };

    private static string[] GetDebugModeEnabled(AzuriteTestContainerConfig configuration)
    {
        var debugLogFilePath = Path.Combine(AzuriteTestContainerConfig.DefaultWorkspaceDirectoryPath, "debug.log");
        return configuration.DebugModeEnabled ? new[] { "--debug", debugLogFilePath } : null;
    }

    private static string GetSilentModeEnabled(AzuriteTestContainerConfig configuration) => configuration.SilentModeEnabled ? "--silent" : null;
    private static string GetLooseModeEnabled(AzuriteTestContainerConfig configuration) => configuration.LooseModeEnabled ? "--loose" : null;
    private static string GetSkipApiVersionCheckEnabled(AzuriteTestContainerConfig configuration) => configuration.SkipApiVersionCheckEnabled ? "--skipApiVersionCheck" : null;
    private static string GetProductStyleUrlDisabled(AzuriteTestContainerConfig configuration) => configuration.ProductStyleUrlDisabled ? "--disableProductStyleUrl" : null;
}