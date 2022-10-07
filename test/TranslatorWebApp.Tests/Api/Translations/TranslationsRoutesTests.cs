using System.Net;
using System.Net.Http.Json;
using Contracts.Events;
using FluentAssertions;
using Rebus.TestHelpers.Events;
using Snapshooter.Xunit;
using TranslatorWebApp.Api.Infrastructure;
using TranslatorWebApp.Api.Translations;
using TranslatorWebApp.Tests.TestHelpers;
using Xunit;

namespace TranslatorWebApp.Tests.Api.Translations;

public class TranslationsRoutesTests : BaseApiTests
{
    [Fact]
    public async Task GivenTranslation_WhenPostTranslation_ThenReturnsAccepted()
    {
        var response = await GivenTestHost()
            .GetClient()
            .PostAsJsonAsync(
                ApiRoutes.Translations, 
                new PostTranslationRequest("this text should be translated into spanish"));

        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
    }
    
    [Fact]
    public async Task GivenTranslationWithEmptyText_WhenPostTranslation_ThenReturnsBadRequest()
    {
        var response = await GivenTestHost()
            .GetClient()
            .PostAsJsonAsync(
                ApiRoutes.Translations, 
                new PostTranslationRequest(string.Empty));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task GivenTranslationWithNullText_WhenPostTranslation_ThenReturnsBadRequest()
    {
        var response = await GivenTestHost()
            .GetClient()
            .PostAsJsonAsync(
                ApiRoutes.Translations, 
                new PostTranslationRequest(null!));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task GivenTranslation_WhenPostTranslation_ThenTranslationDocumentCreatedEventIsSent()
    {
        await GivenTestHost()
            .GetClient()
            .PostAsJsonAsync(
                ApiRoutes.Translations, 
                new PostTranslationRequest("this text should be translated into spanish"));
        
        FakeBus.Events
            .OfType<MessageSent<TranslationDocumentCreated>>().First()
            .CommandMessage
            .Should().MatchSnapshot();
    }
    
    [Fact]
    public async Task GivenTranslation_WhenPostTranslation_ThenTranslationDocumentIsStored()
    {
        var httpResponse = await GivenTestHost()
            .GetClient()
            .PostAsJsonAsync(
                ApiRoutes.Translations, 
                new PostTranslationRequest("this text should be translated into spanish"));

        var response = await httpResponse.Content.ReadFromJsonAsync<PostTranslationResponse>();
        var translationDoc = await Repository.Find(response!.TranslationId);
        translationDoc.Should().MatchSnapshot();
    }

    [Fact]
    public async Task GivenNoTranslation_WhenGetTranslation_ThenReturnNotFound()
    {
        var response = await GivenTestHost()
            .GetClient()
            .GetAsync($"{ApiRoutes.Translations}/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GivenPendingTranslation_WhenGetTranslation_ThenReturnsTranslationDocument()
    {
        var response = await GivenTestHost()
            .GetClient()
            .GetFromJsonAsync<GetTranslationResponse>($"{ApiRoutes.Translations}/{Guid.NewGuid()}");
        
        response.Should().MatchSnapshot();
    }
    
    [Fact]
    public async Task GivenInProcessTranslation_WhenGetTranslation_ThenReturnsTranslationDocument()
    {
        var response = await GivenTestHost()
            .GetClient()
            .GetFromJsonAsync<GetTranslationResponse>($"{ApiRoutes.Translations}/{Guid.NewGuid()}");
        
        response.Should().MatchSnapshot();
    }
    
    [Fact]
    public async Task GivenCompletedTranslation_WhenGetTranslation_ThenReturnsTranslationDocument()
    {
        var response = await GivenTestHost()
            .GetClient()
            .GetFromJsonAsync<GetTranslationResponse>($"{ApiRoutes.Translations}/{Guid.NewGuid()}");
        
        response.Should().MatchSnapshot();
    }
}