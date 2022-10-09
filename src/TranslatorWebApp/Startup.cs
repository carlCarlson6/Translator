using Azure.CognitiveServices;
using Microsoft.Azure.Storage;
using TranslatorWebApp.Api.Infrastructure;
using TranslatorWebApp.Common.Core;
using TranslatorWebApp.Common.Infrastructure;
using TranslatorWebApp.Common.Infrastructure.AzureStorageTables;
using TranslatorWebApp.Common.Infrastructure.Rebus;
using TranslatorWebApp.TranslatorWorker.Infrastructure;

namespace TranslatorWebApp;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment) =>
        (_configuration, _environment) = (configuration, environment);

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddCoreServices()
            .AddApiTranslationServices()
            .AddTranslatorWorkerServices()
            .AddSwaggerGen()
            .AddEndpointsApiExplorer()
            .AddControllers();
        
        if (!_environment.RunningTests())
            services
                .AddRebusServices(_configuration.GetQueueSettings(), CloudStorageAccount.DevelopmentStorageAccount) // TODO - get configuration
                .AddAzureStorageTableClient("", "") // TODO - get configuration
                .AddAzureCognitiveServices(_configuration); 
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