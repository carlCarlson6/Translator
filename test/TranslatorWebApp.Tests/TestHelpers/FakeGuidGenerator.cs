using TranslatorWebApp.Common.Core;

namespace TranslatorWebApp.Tests.TestHelpers;

public class FakeGuidGenerator : IGuidGenerator
{
    public Guid New() => Guid.Parse("a4395f86-cab1-406f-acd1-274c27894b93");

    public Guid Parse(string uuid) => Guid.Parse(uuid);
}