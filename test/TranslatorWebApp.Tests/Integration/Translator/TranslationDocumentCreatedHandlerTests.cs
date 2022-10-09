using Azure.CognitiveServices;
using Azure.CognitiveServices.Language;
using Azure.CognitiveServices.Language.Models;
using Contracts.Events;
using FluentAssertions;
using NSubstitute;
using Rebus.Handlers;
using Rebus.TestHelpers.Events;
using Snapshooter.Xunit;
using TranslatorWebApp.Common.Core;
using TranslatorWebApp.Common.Core.Errors;
using TranslatorWebApp.Common.Infrastructure.AzureStorageTables;
using TranslatorWebApp.Tests.TestHelpers;
using TranslatorWebApp.TranslatorWorker.LanguageIdentification;
using Xunit;

namespace TranslatorWebApp.Tests.Integration.Translator;

public class TranslationDocumentCreatedHandlerTests : TestWithAzurite
{
    private readonly IAzureLanguageApi _languageApiMock = Substitute.For<IAzureLanguageApi>();
    
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

        await BuildHandler().Handle(new TranslationDocumentCreated(doc.Id, doc.OriginalText.ToString()));

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
        
        await BuildHandler().Handle(new TranslationDocumentCreated(doc.Id, doc.OriginalText.ToString()));
        
        FakeBus.Events
            .OfType<MessageSent<LanguageCouldNotBeDetected>>().First()
            .CommandMessage
            .Should().MatchSnapshot();
    }
    
    
    [Fact]
    public async Task GivenSpanishText_WhenHandleTranslationDocumentCreated_ThenDocumentTranslatedEventIsSent()
    {
        _languageApiMock.DetectLanguage(Arg.Any<IEnumerable<DetectLanguageRequest>>())
            .Returns(new List<LanguageDetectionResponse> { new("es", 1, true, true) });
        var doc = new TranslationDocument(
            FakeGuidGenerator.New(),
            new TranslationText("esto es un texto en español"),
            null,
            Status.Pending);
        await (await InitTableClientAsync()).AddEntityAsync(TranslationDocumentTableEntity.From(doc));
        
        await BuildHandler().Handle(new TranslationDocumentCreated(doc.Id, doc.OriginalText.ToString()));
        
        FakeBus.Events
            .OfType<MessageSent<DocumentTranslated>>().First()
            .CommandMessage
            .Should().MatchSnapshot();
    }
    
    [Fact]
    public async Task GivenEnglishText_WhenHandleTranslationDocumentCreated_ThenDocumentLanguageDetectedEventIsSent()
    {
        _languageApiMock.DetectLanguage(Arg.Any<IEnumerable<DetectLanguageRequest>>())
            .Returns(new List<LanguageDetectionResponse>
            {
                new("en", 0.8, true, true),
                new("de", 0.2, true, true)
            });
        var doc = new TranslationDocument(
            FakeGuidGenerator.New(),
            new TranslationText("this text should be translated into spanish"),
            null,
            Status.Pending);
        await (await InitTableClientAsync()).AddEntityAsync(TranslationDocumentTableEntity.From(doc));
        
        await BuildHandler().Handle(new TranslationDocumentCreated(doc.Id, doc.OriginalText.ToString()));
        
        FakeBus.Events
            .OfType<MessageSent<DocumentLanguageDetected>>().First()
            .CommandMessage
            .Should().MatchSnapshot();
    }
    
    [Fact]
    public async Task GivenNoTranslationDocument_WhenHandleTranslationDocumentCreated_ThenTranslationDocumentNotFoundIsThrown()
    {
        await InitTableClientAsync();
        var act = () => BuildHandler().Handle(new TranslationDocumentCreated(FakeGuidGenerator.New(), "some-text"));
        await act.Should().ThrowAsync<TranslationDocumentNotFound>();
    }

    private IHandleMessages<TranslationDocumentCreated> BuildHandler() => new TranslationDocumentCreatedHandler(
        new AzureStorageTableTranslationDocumentsRepository(GivenTableClient()), 
        FakeBus,
        new AzureLanguageIdentifier(_languageApiMock));
}