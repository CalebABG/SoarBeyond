using MediatR;
using SoarBeyond.Domain.Providers.Interfaces;
using SoarBeyond.Shared.Dto;

namespace SoarBeyond.Domain.MediatR.Journals
{
    public class CreateJournalRequest
        : IRequest<Journal>
    {
        public int UserId { get; init; }
        public Journal Journal { get; init; }
    }

    public class CreateJournalRequestHandler
        : IRequestHandler<CreateJournalRequest, Journal>
    {
        private readonly IJournalProvider _journalProvider;

        public CreateJournalRequestHandler(IJournalProvider journalProvider)
        {
            _journalProvider = journalProvider;
        }

        public async Task<Journal> Handle(CreateJournalRequest request, CancellationToken cancellationToken)
        {
            return await _journalProvider.CreateAsync(request);
        }
    }
}