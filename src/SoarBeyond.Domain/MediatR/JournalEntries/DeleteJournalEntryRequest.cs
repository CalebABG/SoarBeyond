using MediatR;
using SoarBeyond.Domain.Providers.Interfaces;

namespace SoarBeyond.Domain.MediatR.JournalEntries;

public class DeleteJournalEntryRequest : IRequest<bool>
{
    public int UserId { get; init; }
    public int JournalId { get; init; }
    public int JournalEntryId { get; init; }
}

public class DeleteJournalEntryRequestHandler
    : IRequestHandler<DeleteJournalEntryRequest, bool>
{
    private readonly IJournalEntryProvider _journalEntryProvider;

    public DeleteJournalEntryRequestHandler(IJournalEntryProvider journalEntryProvider)
    {
        _journalEntryProvider = journalEntryProvider;
    }

    public async Task<bool> Handle(DeleteJournalEntryRequest request, CancellationToken cancellationToken)
    {
        return await _journalEntryProvider.DeleteAsync(request);
    }
}