using Azure.Data.Tables;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace TranslatorWebApp.Tests.TestHelpers;

public static class TestingExtensions
{
    public static HttpClient GetClient(this IWebHost webHost) => webHost.GetTestClient();

    public static TableClient GetTableClient(this IWebHost webHost) => webHost.Services.GetRequiredService<TableClient>();
}