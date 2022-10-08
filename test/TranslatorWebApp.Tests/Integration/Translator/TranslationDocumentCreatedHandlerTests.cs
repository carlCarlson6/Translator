using Contracts.Events;
using FluentAssertions;
using Rebus.Handlers;
using Rebus.TestHelpers.Events;
using Snapshooter.Xunit;
using TranslatorWebApp.Api.Translations.Core;
using TranslatorWebApp.Api.Translations.Infrastructure;
using TranslatorWebApp.Tests.TestHelpers;
using Xunit;

namespace TranslatorWebApp.Tests.Integration.Translator;

public class TranslationDocumentCreatedHandlerTests : TestWithAzurite
{
    private readonly IHandleMessages<TranslationDocumentCreated> _sut = null!;

    [Fact]
    public async Task WhenHandleTranslationDocumentCreated_ThenDocStatusIsUpdatedToInProcess()
    {
        var tableClient = await InitTableClientAsync();
        var doc = new TranslationDocument(
            FakeGuidGenerator.New(),
            new TranslationText("this text should be translated into spanish"),
            null,
            Status.Pending);
        await tableClient.AddEntityAsync(TranslationDocumentTableEntity.From(doc));

        await _sut.Handle(new TranslationDocumentCreated(doc.Id, doc.OriginalText.ToString()));

        var response = await tableClient.GetEntityAsync<TranslationDocumentTableEntity>("Translation", doc.Id.ToString());
        response.Value.Status.Should().Be(Status.InProcess);
    }

    [Fact]
    public async Task GivenTextWithUnknownLanguage_WhenHandleTranslationDocumentCreated_ThenLanguageCouldNotBeDetectedEventIsSent()
    {
        var doc = new TranslationDocument(
            FakeGuidGenerator.New(),
            new TranslationText("this text should be translated into spanish"),
            null,
            Status.Pending);
        await (await InitTableClientAsync()).AddEntityAsync(TranslationDocumentTableEntity.From(doc));
        
        await _sut.Handle(new TranslationDocumentCreated(doc.Id, doc.OriginalText.ToString()));
        
        FakeBus.Events
            .OfType<MessageSent<LanguageCouldNotBeDetected>>().First()
            .CommandMessage
            .Should().MatchSnapshot();
    }
    
    
    [Fact]
    public async Task GivenSpanishText_WhenHandleTranslationDocumentCreated_ThenDocumentTranslatedEventIsSent()
    {
        var doc = new TranslationDocument(
            FakeGuidGenerator.New(),
            new TranslationText("esto es un texto en español"),
            null,
            Status.Pending);
        await (await InitTableClientAsync()).AddEntityAsync(TranslationDocumentTableEntity.From(doc));
        
        await _sut.Handle(new TranslationDocumentCreated(doc.Id, doc.OriginalText.ToString()));
        
        FakeBus.Events
            .OfType<MessageSent<DocumentTranslated>>().First()
            .CommandMessage
            .Should().MatchSnapshot();
    }
    
    [Fact]
    public async Task GivenEnglishText_WhenHandleTranslationDocumentCreated_ThenDocumentLanguageDetectedEventIsSent()
    {
        var doc = new TranslationDocument(
            FakeGuidGenerator.New(),
            new TranslationText("esto es un texto en español"),
            null,
            Status.Pending);
        await (await InitTableClientAsync()).AddEntityAsync(TranslationDocumentTableEntity.From(doc));
        
        await _sut.Handle(new TranslationDocumentCreated(doc.Id, doc.OriginalText.ToString()));
        
        FakeBus.Events
            .OfType<MessageSent<DocumentLanguageDetected>>().First()
            .CommandMessage
            .Should().MatchSnapshot();
    }
}