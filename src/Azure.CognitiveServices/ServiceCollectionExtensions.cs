using Microsoft.Extensions.DependencyInjection;

namespace Azure.CognitiveServices;

public static class ServiceCollectionExtensions
{
    // TODO - get configuration
    public static IServiceCollection AddAzureCognitiveServices(this IServiceCollection services)
    {
        services.AddHttpClient();
        return services;
    }
}