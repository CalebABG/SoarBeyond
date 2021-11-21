using MediatR;
using SoarBeyond.Domain.Providers.Interfaces;
using SoarBeyond.Shared.Dto;

namespace SoarBeyond.Domain.MediatR.JournalEntries;

public class CreateJournalEntryRequest
    : IRequest<JournalEntry>
{
    public int UserId { get; init; }
    public int JournalId { get; init; }
    public JournalEntry JournalEntry { get; init; }
}

public class CreateJournalEntryRequestHandler
    : IRequestHandler<CreateJournalEntryRequest, JournalEntry>
{
    private readonly IJournalEntryProvider _journalEntryProvider;

    public CreateJournalEntryRequestHandler(IJournalEntryProvider journalEntryProvider)
    {
        _journalEntryProvider = journalEntryProvider;
    }

    public async Task<JournalEntry> Handle(CreateJournalEntryRequest request, CancellationToken cancellationToken)
    {
        return await _journalEntryProvider.CreateAsync(request);
    }
}