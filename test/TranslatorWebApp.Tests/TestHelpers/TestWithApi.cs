using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Bus;
using TranslatorWebApp.Api;
using TranslatorWebApp.Api.Infrastructure;
using TranslatorWebApp.Shared;
using TranslatorWebApp.Shared.Infrastructure.AzureStorageTables;

namespace TranslatorWebApp.Tests.TestHelpers;

public class TestWithApi : TestWithAzurite
{
    protected async Task<IWebHost> GivenTestHost()
    {
        var host = WebHost
            .CreateDefaultBuilder()
            .UseStartup<Startup>()
            .UseTestServer()
            .UseEnvironment(WebExtensions.ApiTestsEnvironmentName)
            .ConfigureAppConfiguration(builder => builder.AddEnvironmentVariables())
            .ConfigureTestServices(services => services
                .AddSingleton<IBus>(FakeBus)
                .AddSingleton(GivenTableClient())
                .AddSingleton<IGuidGenerator, FakeGuidGenerator>())
            .UseDefaultServiceProvider((_, options) =>
            {
                options.ValidateScopes = true;
                options.ValidateOnBuild = true;
            })
            .Start();

        await host.GetTableClient().CreateIfNotExistsAsync();
        return host;
    }
}