using MediatR;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Providers.Interfaces;

namespace SoarBeyond.Domain.Mediator.Journals;

public sealed record GetAllJournalsRequest(int UserId)
    : IRequest<IEnumerable<Journal>>;

internal sealed class GetAllJournalsRequestHandler
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