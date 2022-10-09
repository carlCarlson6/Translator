namespace TranslatorWebApp.Common.Core.Errors;

public class ApiError : Exception
{
    public readonly int Code;
    public readonly string Name;

    protected ApiError(int code, string message, string name) : base(message) => (Code, Name) = (code, name);
}