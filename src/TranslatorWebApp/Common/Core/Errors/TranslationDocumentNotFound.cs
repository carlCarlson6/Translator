using System.Net;

namespace TranslatorWebApp.Common.Core.Errors;

public class TranslationDocumentNotFound : ApiError
{
    public TranslationDocumentNotFound(Guid docId) : base(
        (int)HttpStatusCode.NotFound, 
        $"translation document [{docId}] not found", 
        nameof(TranslationDocumentNotFound)) { }
}