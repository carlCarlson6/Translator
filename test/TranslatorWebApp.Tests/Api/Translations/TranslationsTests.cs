using System.Net;
using System.Net.Http.Json;
using Contracts.Events;
using FluentAssertions;
using Rebus.TestHelpers.Events;
using Snapshooter.Xunit;
using TranslatorWebApp.Api.Infrastructure;
using TranslatorWebApp.Api.Translations;
using TranslatorWebApp.Api.Translations.Messages;
using TranslatorWebApp.Common.Core;
using TranslatorWebApp.Common.Core.Errors;
using TranslatorWebApp.Common.Infrastructure.AzureStorageTables;
using TranslatorWebApp.Tests.TestHelpers;
using Xunit;

namespace TranslatorWebApp.Tests.Api.Translations;

public class TranslationsTests : TestWithApi
{
    [Fact]
    public async Task GivenTranslation_WhenPostTranslation_ThenReturnsAccepted()
    {
        var host = await GivenTestHost();
        var httpResponse = await host.GetClient().PostAsJsonAsync(
            ApiRoutes.Translations, 
            new PostTranslationRequest("this text should be translated into spanish"));

        httpResponse.StatusCode.Should().Be(HttpStatusCode.Accepted);
        var response = await httpResponse.Content.ReadAsAsync<PostTranslationResponse>();
        response.Should().MatchSnapshot();
    }
    
    [Fact]
    public async Task GivenTranslationWithEmptyText_WhenPostTranslation_ThenReturnsBadRequest()
    {
        var host = await GivenTestHost();
        var response = await host.GetClient().PostAsJsonAsync(
            ApiRoutes.Translations, 
            new PostTranslationRequest(string.Empty));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        errorResponse.Should().MatchSnapshot();
    }
    
    [Fact]
    public async Task GivenTranslationWithNullText_WhenPostTranslation_ThenReturnsBadRequest()
    {
        var host = await GivenTestHost();
        var response = await host.GetClient().PostAsJsonAsync(
            ApiRoutes.Translations, 
            new PostTranslationRequest(null!));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task GivenTranslation_WhenPostTranslation_ThenTranslationDocumentCreatedEventIsSent()
    {
        var host = await GivenTestHost();
        await host.GetClient().PostAsJsonAsync(
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
        var host = await GivenTestHost();
        var httpResponse = await host.GetClient().PostAsJsonAsync(
            ApiRoutes.Translations, 
            new PostTranslationRequest("this text should be translated into spanish"));
        var response = await httpResponse.Content.ReadFromJsonAsync<PostTranslationResponse>();
        
        var tableEntityResponse = await host.GetTableClient()
            .GetEntityAsync<TranslationDocumentTableEntity>("Translation", response!.TranslationId.ToString());
        tableEntityResponse.Value.Should().MatchSnapshot(options => 
            options.IgnoreField("Timestamp"));
    }

    [Fact]
    public async Task GivenNoTranslation_WhenGetTranslation_ThenReturnNotFound()
    { 
        var host = await GivenTestHost();
        var response = await host.GetClient().GetAsync($"{ApiRoutes.Translations}/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GivenPendingTranslation_WhenGetTranslation_ThenReturnsTranslationDocument()
    {
        var host = await GivenTestHost();
        await GivenTableClient().AddEntityAsync(TranslationDocumentTableEntity.From(
            new TranslationDocument(
            FakeGuidGenerator.New(),
            new TranslationText("this text should be translated into spanish"),
            null,
            Status.Pending)));

        var response = await host
            .GetClient()
            .GetFromJsonAsync<GetTranslationResponse>($"{ApiRoutes.Translations}/{FakeGuidGenerator.New()}");
        
        response.Should().MatchSnapshot();
    }
    
    [Fact]
    public async Task GivenInProcessTranslation_WhenGetTranslation_ThenReturnsTranslationDocument()
    {
        var host = await GivenTestHost();
        await GivenTableClient().AddEntityAsync(TranslationDocumentTableEntity.From(new TranslationDocument(
            FakeGuidGenerator.New(),
            new TranslationText("this text should be translated into spanish"),
            null,
            Status.InProcess)));
        
        var response = await host
            .GetClient()
            .GetFromJsonAsync<GetTranslationResponse>($"{ApiRoutes.Translations}/{FakeGuidGenerator.New()}");
        
        response.Should().MatchSnapshot();
    }
    
    [Fact]
    public async Task GivenCompletedTranslation_WhenGetTranslation_ThenReturnsTranslationDocument()
    {
        var host = await GivenTestHost();
        await GivenTableClient().AddEntityAsync(TranslationDocumentTableEntity.From(new TranslationDocument(
            FakeGuidGenerator.New(),
            new TranslationText("this text should be translated into spanish"),
            new TranslationText("esta es la traduccion"),
            Status.Completed)));
        
        var response = await host
            .GetClient()
            .GetFromJsonAsync<GetTranslationResponse>($"{ApiRoutes.Translations}/{FakeGuidGenerator.New()}");
        
        response.Should().MatchSnapshot();
    }
}