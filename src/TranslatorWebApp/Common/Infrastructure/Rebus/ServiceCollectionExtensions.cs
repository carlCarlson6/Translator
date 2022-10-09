using Contracts.Events;
using Microsoft.Azure.Storage;
using Newtonsoft.Json;
using Rebus.Config;
using Rebus.Retry.Simple;
using Rebus.Routing.TypeBased;
using Rebus.Serialization.Json;

namespace TranslatorWebApp.Common.Infrastructure.Rebus;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRebusServices(this IServiceCollection services, RebusQueueSettings queueSettings, CloudStorageAccount cloudStorageAccount) => services
        .AutoRegisterHandlersFromAssemblyOf<Program>()
        .AddRebus((configurer, _) => configurer
            .ConfigureRebus()
            .Transport(t =>
                t.UseAzureStorageQueues(
                    cloudStorageAccount, 
                    queueSettings.TranslatorWebAppQueue, 
                    new AzureStorageQueuesTransportOptions
                    {
                        AutomaticallyCreateQueues = true
                    }))
            .Routing(standardConfigurer =>
                standardConfigurer.TypeBased()
                    .Map<DocumentLanguageDetected>(queueSettings.TranslatorWebAppQueue)
                    .Map<DocumentTranslated>(queueSettings.TranslatorWebAppQueue)
                    .Map<LanguageCouldNotBeDetected>(queueSettings.TranslatorWebAppQueue)
                    .Map<TranslationDocumentCreated>(queueSettings.TranslatorWebAppQueue)));
    
    public static RebusQueueSettings GetQueueSettings(this IConfiguration configuration)
    {
        var queueSettings = new RebusQueueSettings();
        configuration.Bind(queueSettings);
        return queueSettings;
    }

    private static RebusConfigurer ConfigureRebus(this RebusConfigurer configurer) => configurer
        .Serialization(opt =>
            opt.UseNewtonsoftJson(new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }))
        .Options(opt =>
        {
            opt.SetNumberOfWorkers(1);
            opt.SimpleRetryStrategy(maxDeliveryAttempts: 2);
        });
}