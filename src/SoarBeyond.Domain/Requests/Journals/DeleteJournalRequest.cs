using MediatR;
using SoarBeyond.Domain.Providers.Interfaces;

namespace SoarBeyond.Domain.Requests.Journals;

public sealed record DeleteJournalRequest(int UserId, int JournalId)
    : IRequest<bool>;

internal sealed class DeleteJournalRequestHandler
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