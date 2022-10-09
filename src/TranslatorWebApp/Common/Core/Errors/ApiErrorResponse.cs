namespace TranslatorWebApp.Common.Core.Errors;

public record ApiErrorResponse(string ErrorMessage, string ErrorName)
{
    public static ApiErrorResponse FromApiError(ApiError error) => new(error.Message, error.Name);
}