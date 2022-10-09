namespace Contracts.Events;

public record DocumentLanguageDetected(Guid DocumentId, string Text, string LanguageCode);