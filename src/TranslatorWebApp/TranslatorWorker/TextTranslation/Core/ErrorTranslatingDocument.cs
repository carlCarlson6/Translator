using System.Net;
using TranslatorWebApp.Common.Core.Errors;

namespace TranslatorWebApp.TranslatorWorker.TextTranslation.Core;

public class ErrorTranslatingDocument : ApiError
{
    public ErrorTranslatingDocument(Guid documentId, string text, string languageCode) : base(
        (int)HttpStatusCode.InternalServerError, 
        $"could not translate document [{documentId}] - [{languageCode}] - [{text}]", 
        nameof(ErrorTranslatingDocument)) { }
}