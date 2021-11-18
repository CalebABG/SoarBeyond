using SoarBeyond.Domain.Providers.Interfaces;

namespace SoarBeyond.Domain.Providers;

public class SoarBeyondDbProvider : ISoarBeyondDbProvider
{
    public IJournalProvider JournalProvider { get; }
    public IJournalEntryProvider JournalEntryProvider { get; }
    public IThoughtProvider ThoughtProvider { get; }

    public SoarBeyondDbProvider(
        IJournalProvider journalProvider,
        IJournalEntryProvider journalEntryProvider,
        IThoughtProvider thoughtProvider)
    {
        JournalProvider = journalProvider;
        JournalEntryProvider = journalEntryProvider;
        ThoughtProvider = thoughtProvider;
    }
}