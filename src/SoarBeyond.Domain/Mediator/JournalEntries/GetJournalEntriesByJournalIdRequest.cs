using MediatR;
using SoarBeyond.Domain.Providers.Interfaces;
using SoarBeyond.Shared.Dto;

namespace SoarBeyond.Domain.Mediator.JournalEntries;

public class GetJournalEntriesByJournalIdRequest
    : IRequest<IEnumerable<JournalEntry>>
{
    public int UserId { get; init; }
    public int JournalId { get; init; }
}

public class GetJournalEntriesByJournalIdRequestHandler
    : IRequestHandler<GetJournalEntriesByJournalIdRequest, IEnumerable<JournalEntry>>
{
    private readonly IJournalEntryProvider _journalEntryProvider;

    public GetJournalEntriesByJournalIdRequestHandler(IJournalEntryProvider journalEntryProvider)
    {
        _journalEntryProvider = journalEntryProvider;
    }

    public async Task<IEnumerable<JournalEntry>> Handle(GetJournalEntriesByJournalIdRequest request, CancellationToken cancellationToken)
    {
        return await _journalEntryProvider.GetByJournalIdAsync(request);
    }
}