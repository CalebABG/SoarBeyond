using MediatR;
using SoarBeyond.Domain.Providers.Interfaces;

namespace SoarBeyond.Domain.Requests.Journals;

public sealed record UpdateJournalFavoriteStatusRequest(int UserId, int JournalId, bool Favorited)
    : IRequest<bool>;

internal sealed class UpdateJournalFavoriteStatusRequestHandler
    : IRequestHandler<UpdateJournalFavoriteStatusRequest, bool>
{
    private readonly IJournalProvider _journalProvider;

    public UpdateJournalFavoriteStatusRequestHandler(IJournalProvider journalProvider)
    {
        _journalProvider = journalProvider;
    }

    public async Task<bool> Handle(UpdateJournalFavoriteStatusRequest request, CancellationToken cancellationToken)
    {
        return await _journalProvider.UpdateFavoriteStatusAsync(request);
    }
}