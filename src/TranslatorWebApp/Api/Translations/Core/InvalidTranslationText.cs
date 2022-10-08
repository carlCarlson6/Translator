using System.Net;
using TranslatorWebApp.Api.Common;

namespace TranslatorWebApp.Api.Translations.Core;

public class InvalidTranslationText : ApiError
{
    public InvalidTranslationText() : base(
        (int)HttpStatusCode.BadRequest, 
        "text can not be null or empty", 
        nameof(InvalidTranslationText)) { }
}