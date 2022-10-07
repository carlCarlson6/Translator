using System.Net.Http.Json;
using FluentAssertions;
using Snapshooter.Xunit;
using TranslatorWebApp.Api.Health;
using TranslatorWebApp.Api.Infrastructure;
using Xunit;

namespace TranslatorWebApp.Tests.Api.Health;

public class HealthTests : BaseApiTests
{
    [Fact]
    public async Task WhenGetHealthRoute_ThenReturnsHelloWorldResponse()
    {
        var response = await GivenTestHost()
            .GetClient()
            .GetFromJsonAsync<HealthControllerResponse>(ApiRoutes.Health);
        response.Should().MatchSnapshot();
    }

    [Fact]
    public async Task WhenGetHealthRouteWithName_ThenReturnsHelloWorldResponse()
    {
        var response = await GivenTestHost()
            .GetClient()
            .GetFromJsonAsync<HealthControllerResponse>($"{ApiRoutes.Health}/carl");
        response.Should().MatchSnapshot();
    }
}