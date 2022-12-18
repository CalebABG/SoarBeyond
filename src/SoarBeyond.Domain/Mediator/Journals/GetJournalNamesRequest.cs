using MediatR;
using SoarBeyond.Data;
using SoarBeyond.Domain.Providers.Interfaces;
using SoarBeyond.Shared.Poco;

namespace SoarBeyond.Domain.Mediator.Journals;

public sealed record GetJournalNamesRequest(int UserId)
    : IRequest<HashSet<JournalName>>;

internal sealed class GetJournalNamesRequestHandler
    : IRequestHandler<GetJournalNamesRequest, IEnumerable<JournalName>>
{
    private readonly IJournalProvider _journalProvider;

    public GetJournalNamesRequestHandler(IJournalProvider journalProvider)
    {
        _journalProvider = journalProvider;
    }

    public Task<IEnumerable<JournalName>> Handle(GetJournalNamesRequest request, CancellationToken cancellationToken)
    {
        return _journalProvider.GetNamesAsync(request);
    }
}