using Azure.CognitiveServices;
using Microsoft.Azure.Storage;
using TranslatorWebApp.Api.Infrastructure;
using TranslatorWebApp.Api.Translations.Infrastructure;
using TranslatorWebApp.Shared;
using TranslatorWebApp.Shared.Infrastructure.AzureStorageTables;
using TranslatorWebApp.Shared.Infrastructure.Rebus;

namespace TranslatorWebApp.Api;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment) =>
        (_configuration, _environment) = (configuration, environment);

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddSingleton<IGuidGenerator, CsharpGuidGenerator>()
            .AddApiTranslationServices()
            .AddSwaggerGen()
            .AddEndpointsApiExplorer()
            .AddControllers();
        
        if (!_environment.RunningTests())
            services
                .AddRebusServices(
                    _configuration.GetQueueSettings(),
                    CloudStorageAccount.DevelopmentStorageAccount) // TODO - get configuration
                .AddAzureStorageTableClient(
                    "",
                    "") // TODO - get configuration
                .AddAzureCognitiveServices(); 
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app
            .UseHttpsRedirection()
            .UseRouting()
            .UseEndpoints(builder => builder.MapControllers());
    }
}