using System.Net;

namespace TranslatorWebApp.Common.Core.Errors;

public class InvalidTranslationText : ApiError
{
    public InvalidTranslationText() : base(
        (int)HttpStatusCode.BadRequest, 
        "text can not be null or empty", 
        nameof(InvalidTranslationText)) { }
}