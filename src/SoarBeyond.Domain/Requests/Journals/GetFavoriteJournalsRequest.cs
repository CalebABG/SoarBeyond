using MediatR;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Providers.Interfaces;

namespace SoarBeyond.Domain.Requests.Journals;

public sealed record GetFavoriteJournalsRequest(int UserId)
    : IRequest<IEnumerable<Journal>>;

internal sealed class GetFavoriteJournalsRequestHandler
    : IRequestHandler<GetFavoriteJournalsRequest, IEnumerable<Journal>>
{
    private readonly IJournalProvider _journalProvider;

    public GetFavoriteJournalsRequestHandler(IJournalProvider journalProvider)
    {
        _journalProvider = journalProvider;
    }
    
    public async Task<IEnumerable<Journal>> Handle(GetFavoriteJournalsRequest request, CancellationToken cancellationToken)
    {
        return await _journalProvider.GetFavoritesAsync(request);
    }
}