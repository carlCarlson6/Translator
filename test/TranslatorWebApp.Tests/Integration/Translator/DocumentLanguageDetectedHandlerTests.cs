using System.Collections.Immutable;
using Azure.CognitiveServices;
using Azure.CognitiveServices.Language.Models;
using Contracts.Events;
using FluentAssertions;
using NSubstitute;
using Rebus.Handlers;
using Rebus.TestHelpers.Events;
using Snapshooter.Xunit;
using TranslatorWebApp.Tests.TestHelpers;
using TranslatorWebApp.TranslatorWorker.TextTranslation;
using TranslatorWebApp.TranslatorWorker.TextTranslation.Core;
using Xunit;

namespace TranslatorWebApp.Tests.Integration.Translator;

public class DocumentLanguageDetectedHandlerTests : TestWithAzurite
{
    private readonly IAzureLanguageApi _languageApiMock = Substitute.For<IAzureLanguageApi>();

    private readonly DocumentLanguageDetected _event = new(
        new FakeGuidGenerator().New(), "this text should be translated into spanish", "en");
    
    [Fact]
    public async Task WhenHandleDocumentLanguageDetected_AndTranslationIsOk_ThenDocumentTranslatedEventIsSent()
    {
        _languageApiMock.Translate(Arg.Any<TranslationRequest>())
            .Returns(new List<TranslationResponse>
            {
                new(new List<AzureTranslation>
                {
                    new("esta es la traduccion del text", "es")
                })
            });

        await GivenEventHandler().Handle(_event);
        
        FakeBus.Events
            .OfType<MessageSent<DocumentTranslated>>().First()
            .CommandMessage
            .Should().MatchSnapshot();
    }

    [Fact]
    public async Task WhenHandleDocumentLanguageDetected_AndTranslationIsKo_ThenErrorTranslatingDocumentIsThrown()
    {
        _languageApiMock.Translate(Arg.Any<TranslationRequest>())
            .Returns(ImmutableList<TranslationResponse>.Empty);
        
        var act = () => GivenEventHandler().Handle(_event);
        await act.Should().ThrowAsync<ErrorTranslatingDocument>();
    }
    
    private IHandleMessages<DocumentLanguageDetected> GivenEventHandler() => new DocumentLanguageDetectedHandler(
        new AzureTextTranslator(_languageApiMock), FakeBus);
}