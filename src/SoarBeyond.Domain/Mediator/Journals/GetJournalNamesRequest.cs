using MediatR;
using SoarBeyond.Data;
using SoarBeyond.Domain.Providers.Interfaces;
using SoarBeyond.Shared.Poco;

namespace SoarBeyond.Domain.Mediator.Journals;

public class GetJournalNamesRequest : IRequest<HashSet<JournalName>>
{
    public int UserId { get; init; }
}

public class GetJournalNamesRequestHandler
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