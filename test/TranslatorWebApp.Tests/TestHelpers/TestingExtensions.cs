using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace TranslatorWebApp.Tests.TestHelpers;

public static class TestingExtensions
{
    public static HttpClient GetClient(this IWebHost webHost) => webHost.GetTestClient();
}