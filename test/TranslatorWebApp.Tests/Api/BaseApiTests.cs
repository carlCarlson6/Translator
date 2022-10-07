using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using TranslatorWebApp.Api;

namespace TranslatorWebApp.Tests.Api;

public class BaseApiTests
{
    protected IWebHost GivenTestHost() => WebHost
        .CreateDefaultBuilder()
        .UseStartup<Startup>()
        .UseTestServer()
        .ConfigureAppConfiguration(builder => builder.AddEnvironmentVariables())
        .ConfigureTestServices(_ => {})
        .UseDefaultServiceProvider((_, options) =>
        {
            // makes sure DI lifetimes and scopes don't have common issues
            options.ValidateScopes = true;
            options.ValidateOnBuild = true;
        })
        .Start();
}

public static class TestingExtensions
{
    public static HttpClient GetClient(this IWebHost webHost) => webHost.GetTestClient();
}