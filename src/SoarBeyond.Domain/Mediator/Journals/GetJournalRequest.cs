using MediatR;
using SoarBeyond.Domain.Providers.Interfaces;
using SoarBeyond.Shared.Dto;

namespace SoarBeyond.Domain.Mediator.Journals;

public class GetJournalRequest : IRequest<Journal>
{
    public int UserId { get; init; }
    public int JournalId { get; init; }
}

public class GetJournalRequestHandler
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