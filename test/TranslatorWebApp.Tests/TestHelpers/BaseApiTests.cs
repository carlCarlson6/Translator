using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.TestHelpers;
using TranslatorWebApp.Api;
using TranslatorWebApp.Api.Translations;
using TranslatorWebApp.Api.Translations.Core;

namespace TranslatorWebApp.Tests.TestHelpers;

public class BaseApiTests
{
    protected readonly FakeBus FakeBus = new();
    protected readonly ITranslationDocumentsRepository Repository = null!;
    
    protected IWebHost GivenTestHost() => WebHost
        .CreateDefaultBuilder()
        .UseStartup<Startup>()
        .UseTestServer()
        .ConfigureAppConfiguration(builder => builder.AddEnvironmentVariables())
        .ConfigureTestServices(services => 
            services.AddSingleton(FakeBus))
        .UseDefaultServiceProvider((_, options) =>
        {
            // makes sure DI lifetimes and scopes don't have common issues
            options.ValidateScopes = true;
            options.ValidateOnBuild = true;
        })
        .Start();
}