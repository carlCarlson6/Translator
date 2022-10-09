using Azure.CognitiveServices.Language;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static System.String;

namespace Azure.CognitiveServices;

public static class ServiceCollectionExtensions
{
    // TODO - get configuration
    public static IServiceCollection AddAzureCognitiveServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient()
            .AddHttpClient<IAzureLanguageApi, AzureLanguageHttpApi>((_, client) => client
            .ConfigureAzureTranslator(configuration.GetTranslatorUrl(), "", ""));
        
        return services;
    }
    
    private static string GetTranslatorUrl(this IConfiguration configuration) => configuration["AzureCognitiveServices:TranslatorUrl"];
}

public static class HttpClientExtensions
{
    public static void ConfigureAzureTranslator(this HttpClient client, string baseUrl, string key, string region)
    {
        if (IsNullOrWhiteSpace(baseUrl))
            throw new ArgumentNullException(nameof(baseUrl), "please make sure you configured abus base URL");
        if (IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key), "please make sure you configured api key");
        
        client.BaseAddress = new Uri(baseUrl);
        client.Timeout = TimeSpan.FromSeconds(60);
        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);
        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Region", region);
    }
}