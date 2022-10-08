using System.Net.Http.Json;
using FluentAssertions;
using Snapshooter.Xunit;
using TranslatorWebApp.Api.Health;
using TranslatorWebApp.Api.Infrastructure;
using TranslatorWebApp.Tests.TestHelpers;
using Xunit;

namespace TranslatorWebApp.Tests.Api.Health;

public class HealthTests : TestWithApi
{
    [Fact]
    public async Task WhenGetHealthRoute_ThenReturnsHelloWorldResponse()
    {
        var host = await GivenTestHost();
        var response = await host
            .GetClient()
            .GetFromJsonAsync<HealthControllerResponse>(ApiRoutes.Health);
        response.Should().MatchSnapshot();
    }

    [Fact]
    public async Task WhenGetHealthRouteWithName_ThenReturnsHelloWorldResponse()
    {
        var host = await GivenTestHost();
        var response = await host
            .GetClient()
            .GetFromJsonAsync<HealthControllerResponse>($"{ApiRoutes.Health}/carl");
        response.Should().MatchSnapshot();
    }
}