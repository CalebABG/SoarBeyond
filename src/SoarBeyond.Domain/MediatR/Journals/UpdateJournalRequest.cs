using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SoarBeyond.Domain.Providers.Interfaces;
using SoarBeyond.Shared.Dto;

namespace SoarBeyond.Domain.MediatR.Journals
{
    public class UpdateJournalRequest
        : IRequest<Journal>
    {
        public int UserId { get; init; }
        public int JournalId { get; init; }
        public Journal Journal { get; init; }
    }

    public class UpdateJournalRequestHandler
        : IRequestHandler<UpdateJournalRequest, Journal>
    {
        private readonly IJournalProvider _journalProvider;

        public UpdateJournalRequestHandler(IJournalProvider journalProvider)
        {
            _journalProvider = journalProvider;
        }

        public async Task<Journal> Handle(UpdateJournalRequest request, CancellationToken cancellationToken)
        {
            return await _journalProvider.UpdateAsync(request);
        }
    }
}