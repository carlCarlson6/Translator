using Contracts.Events;
using FluentAssertions;
using Rebus.Handlers;
using Snapshooter.Xunit;
using TranslatorWebApp.Common.Core;
using TranslatorWebApp.Common.Core.Errors;
using TranslatorWebApp.Common.Infrastructure.AzureStorageTables;
using TranslatorWebApp.Tests.TestHelpers;
using TranslatorWebApp.TranslatorWorker.LanguageIdentification;
using Xunit;

namespace TranslatorWebApp.Tests.Integration.Translator;

public class LanguageCouldNotBeDetectedHandlerTests : TestWithAzurite
{
    [Fact]
    public async Task WhenHandleLanguageCouldNotBeDetectedHandlerTests_ThenDocStatusIsUpdatedToCompleted()
    {
        var tableClient = await InitTableClientAsync();
        var doc = new TranslationDocument(
            FakeGuidGenerator.New(),
            new TranslationText("this text should be translated into spanish"),
            null,
            Status.InProcess);
        await tableClient.AddEntityAsync(TranslationDocumentTableEntity.From(doc));

        await BuildHandler().Handle(new LanguageCouldNotBeDetected(doc.Id));

        var response = await tableClient.GetEntityAsync<TranslationDocumentTableEntity>("Translation", doc.Id.ToString());
        response.Value.Status.Should().Be(Status.Completed);
        response.Value.Should().MatchSnapshot(o => o.IgnoreField(nameof(TranslationDocumentTableEntity.Timestamp)));
    }
    
    [Fact]
    public async Task GivenNoTranslationDocument_WhenHandleLanguageCouldNotBeDetected_ThenTranslationDocumentNotFoundIsThrown()
    {
        await InitTableClientAsync();
        var act = () => BuildHandler().Handle(new LanguageCouldNotBeDetected(FakeGuidGenerator.New()));
        await act.Should().ThrowAsync<TranslationDocumentNotFound>();
    }
    
    private IHandleMessages<LanguageCouldNotBeDetected> BuildHandler() => new LanguageCouldNotBeDetectedHandler(
        new AzureStorageTableTranslationDocumentsRepository(GivenTableClient()));
}