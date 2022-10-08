namespace TranslatorWebApp.Shared;

public interface IGuidGenerator
{
    public Guid New();
    public Guid Parse(string uuid);
}

public class CsharpGuidGenerator : IGuidGenerator 
{
    public Guid New() => Guid.NewGuid();

    public Guid Parse(string uuid) => Guid.Parse(uuid);
}