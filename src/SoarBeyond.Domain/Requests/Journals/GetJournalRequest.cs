using MediatR;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Providers.Interfaces;

namespace SoarBeyond.Domain.Requests.Journals;

public sealed record GetJournalRequest(int UserId, int JournalId)
    : IRequest<Journal>;

internal sealed class GetJournalRequestHandler
    : IRequestHandler<GetJournalRequest, Journal>
{
    private readonly IJournalProvider _journalProvider;

    public GetJournalRequestHandler(IJournalProvider journalProvider)
    {
        _journalProvider = journalProvider;
    }

    public async Task<Journal> Handle(GetJournalRequest request, CancellationToken cancellationToken)
    {
        return await _journalProvider.GetAsync(request);
    }
}