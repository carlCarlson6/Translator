using Contracts.Events;
using FluentAssertions;
using Rebus.Handlers;
using Snapshooter.Xunit;
using TranslatorWebApp.Common.Core;
using TranslatorWebApp.Common.Core.Errors;
using TranslatorWebApp.Common.Infrastructure.AzureStorageTables;
using TranslatorWebApp.Tests.TestHelpers;
using TranslatorWebApp.TranslatorWorker.TextTranslation;
using Xunit;

namespace TranslatorWebApp.Tests.Integration.Translator;

public class DocumentTranslatedHandlerTests : TestWithAzurite
{
    [Fact]
    public async Task WhenHandleDocumentTranslated_ThenDocStatusIsUpdatedToCompleted()
    {
        var tableClient = await InitTableClientAsync();
        var doc = new TranslationDocument(
            FakeGuidGenerator.New(),
            new TranslationText("this text should be translated into spanish"),
            new TranslationText("esta es la traduccion"),
            Status.InProcess);
        await tableClient.AddEntityAsync(TranslationDocumentTableEntity.From(doc));

        await BuildHandler().Handle(new DocumentTranslated(doc.Id, doc.OriginalText.ToString()));

        var response = await tableClient.GetEntityAsync<TranslationDocumentTableEntity>("Translation", doc.Id.ToString());
        response.Value.Status.Should().Be(Status.Completed);
        response.Value.Should().MatchSnapshot(o => o.IgnoreField(nameof(TranslationDocumentTableEntity.Timestamp)));
    }

    [Fact]
    public async Task GivenNoTranslationDocument_WhenHandleDocumentTranslated_ThenTranslationDocumentNotFoundIsThrown()
    {
        await InitTableClientAsync();
        var act = () => BuildHandler().Handle(new DocumentTranslated(FakeGuidGenerator.New(), "some text"));
        await act.Should().ThrowAsync<TranslationDocumentNotFound>();
    }

    private IHandleMessages<DocumentTranslated> BuildHandler() => new DocumentTranslatedHandler(
        new AzureStorageTableTranslationDocumentsRepository(GivenTableClient()));
}