namespace TranslatorWebApp.Api.Translations.Core;

public class TranslationText
{
    private readonly string _value;

    public TranslationText(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidTranslationText();
        _value = value;
    }

    public override string ToString() => _value;
}