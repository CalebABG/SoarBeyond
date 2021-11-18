using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SoarBeyond.Domain.Providers.Interfaces;

namespace SoarBeyond.Domain.MediatR.Journals
{
    public class DeleteJournalRequest
        : IRequest<bool>
    {
        public int UserId { get; init; }
        public int JournalId { get; init; }
    }

    public class DeleteJournalRequestHandler
        : IRequestHandler<DeleteJournalRequest, bool>
    {
        private readonly IJournalProvider _journalProvider;

        public DeleteJournalRequestHandler(IJournalProvider journalProvider)
        {
            _journalProvider = journalProvider;
        }

        public async Task<bool> Handle(DeleteJournalRequest request, CancellationToken cancellationToken)
        {
            return await _journalProvider.DeleteAsync(request);
        }
    }
}