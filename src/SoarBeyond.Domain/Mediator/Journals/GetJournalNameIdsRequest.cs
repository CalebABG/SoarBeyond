using MediatR;
using SoarBeyond.Data;
using SoarBeyond.Domain.Providers.Interfaces;
using SoarBeyond.Shared.Poco;

namespace SoarBeyond.Domain.Mediator.Journals;

public class GetJournalNameIdsRequest : IRequest<HashSet<JournalNameId>>
{
    public int UserId { get; init; }
}

public class GetJournalNameIdsRequestHandler
    : IRequestHandler<GetJournalNameIdsRequest, HashSet<JournalNameId>>
{
    private readonly IJournalProvider _journalProvider;

    public GetJournalNameIdsRequestHandler(IJournalProvider journalProvider)
    {
        _journalProvider = journalProvider;
    }

    public Task<HashSet<JournalNameId>> Handle(GetJournalNameIdsRequest request, CancellationToken cancellationToken)
    {
        return _journalProvider.GetNameIdsAsync(request);
    }
}