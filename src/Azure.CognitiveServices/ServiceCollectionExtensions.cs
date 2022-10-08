using Microsoft.Extensions.DependencyInjection;

namespace Azure.CognitiveServices;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAzureCognitiveServices(this IServiceCollection services) => services;
}