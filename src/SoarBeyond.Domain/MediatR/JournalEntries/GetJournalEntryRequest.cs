using MediatR;
using SoarBeyond.Domain.Providers.Interfaces;
using SoarBeyond.Shared.Dto;

namespace SoarBeyond.Domain.MediatR.JournalEntries;

public class GetJournalEntryRequest
    : IRequest<JournalEntry>
{
    public int UserId { get; init; }
    public int JournalEntryId { get; init; }
}

public class GetJournalEntryRequestHandler
    : IRequestHandler<GetJournalEntryRequest, JournalEntry>
{
    private readonly IJournalEntryProvider _journalEntryProvider;

    public GetJournalEntryRequestHandler(IJournalEntryProvider journalEntryProvider)
    {
        _journalEntryProvider = journalEntryProvider;
    }

    public async Task<JournalEntry> Handle(GetJournalEntryRequest request, CancellationToken cancellationToken)
    {
        return await _journalEntryProvider.GetAsync(request);
    }
}