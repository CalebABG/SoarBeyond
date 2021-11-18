using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SoarBeyond.Domain.Providers.Interfaces;
using SoarBeyond.Shared.Dto;

namespace SoarBeyond.Domain.MediatR.JournalEntries
{
    public class GetAllJournalEntriesRequest
        : IRequest<IEnumerable<JournalEntry>>
    {
        public int UserId { get; init; }
    }

    public class GetAllJournalEntriesRequestHandler
        : IRequestHandler<GetAllJournalEntriesRequest, IEnumerable<JournalEntry>>
    {
        private readonly IJournalEntryProvider _journalEntryProvider;

        public GetAllJournalEntriesRequestHandler(IJournalEntryProvider journalEntryProvider)
        {
            _journalEntryProvider = journalEntryProvider;
        }

        public async Task<IEnumerable<JournalEntry>> Handle(GetAllJournalEntriesRequest request, CancellationToken cancellationToken)
        {
            return await _journalEntryProvider.GetAllAsync(request);
        }
    }
}