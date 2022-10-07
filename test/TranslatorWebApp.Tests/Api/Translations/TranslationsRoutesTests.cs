using Xunit;

namespace TranslatorWebApp.Tests.Api.Translations;

public class TranslationsRoutesTests : BaseApiTests
{
    [Fact]
    public async Task GivenTranslation_WhenPostTranslation_ThenReturns202Accepted()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public async Task GivenTranslation_WhenPostTranslation_ThenNewTranslationDocumentCreatedEventIsSent()
    {
        throw new NotImplementedException();
    }
    
    [Fact]
    public async Task GivenTranslation_WhenPostTranslation_ThenTranslationDocumentIsStored()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public async Task GivenNoTranslation_WhenGetTranslation_ThenReturn404NotFound()
    {
        throw new NotImplementedException();
    }
    
    [Fact]
    public async Task GivenStoredTranslationDocument_WhenGetTranslation_ThenReturns200_AndTranslationDocument()
    {
        throw new NotImplementedException();
    }
}