using MediatR;
using SoarBeyond.Domain.Providers.Interfaces;
using SoarBeyond.Shared.Dto;

namespace SoarBeyond.Domain.MediatR.JournalEntries;

public class UpdateJournalEntryRequest
    : IRequest<JournalEntry>
{
    public int UserId { get; init; }
    public int JournalEntryId { get; init; }
    public JournalEntry JournalEntry { get; init; }
}

public class UpdateJournalEntryRequestHandler
    : IRequestHandler<UpdateJournalEntryRequest, JournalEntry>
{
    private readonly IJournalEntryProvider _journalProvider;

    public UpdateJournalEntryRequestHandler(IJournalEntryProvider journalProvider)
    {
        _journalProvider = journalProvider;
    }

    public async Task<JournalEntry> Handle(UpdateJournalEntryRequest request, CancellationToken cancellationToken)
    {
        return await _journalProvider.UpdateAsync(request);
    }
}