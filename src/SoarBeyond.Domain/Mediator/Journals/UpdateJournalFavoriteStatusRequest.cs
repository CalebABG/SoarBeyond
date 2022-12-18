using MediatR;
using SoarBeyond.Domain.Providers.Interfaces;

namespace SoarBeyond.Domain.Mediator.Journals;

public class UpdateJournalFavoriteStatusRequest : IRequest<bool>
{
    public int UserId { get; init; }
    public int JournalId { get; init; }
    public bool Favored { get; init; }
}

public class UpdateJournalFavoriteStatusRequestHandler
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