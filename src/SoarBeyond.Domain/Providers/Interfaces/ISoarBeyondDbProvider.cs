namespace SoarBeyond.Domain.Providers.Interfaces;

public interface ISoarBeyondDbProvider
{
    IJournalProvider JournalProvider { get; }
    IJournalEntryProvider JournalEntryProvider { get; }
    IThoughtProvider ThoughtProvider { get; }
}