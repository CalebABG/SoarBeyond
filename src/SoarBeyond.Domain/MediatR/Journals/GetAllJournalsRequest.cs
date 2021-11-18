using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SoarBeyond.Domain.Providers.Interfaces;
using SoarBeyond.Shared.Dto;

namespace SoarBeyond.Domain.MediatR.Journals
{
    public class GetAllJournalsRequest
        : IRequest<IEnumerable<Journal>>
    {
        public int UserId { get; init; }
    }

    public class GetAllJournalsRequestHandler
        : IRequestHandler<GetAllJournalsRequest, IEnumerable<Journal>>
    {
        private readonly IJournalProvider _journalProvider;

        public GetAllJournalsRequestHandler(IJournalProvider journalProvider)
        {
            _journalProvider = journalProvider;
        }

        public async Task<IEnumerable<Journal>> Handle(GetAllJournalsRequest request, CancellationToken cancellationToken)
        {
            return await _journalProvider.GetAllAsync(request);
        }
    }
}