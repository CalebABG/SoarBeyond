using MediatR;
using SoarBeyond.Data;
using SoarBeyond.Shared.Poco;

namespace SoarBeyond.Domain.MediatR.Journals;

public class GetJournalNameIdsRequest : IRequest<HashSet<JournalNameId>>
{
    public int UserId { get; init; }
}

public class GetJournalNameIdsRequestHandler
    : IRequestHandler<GetJournalNameIdsRequest, HashSet<JournalNameId>>
{
    private readonly SoarBeyondDbContext _dbContext;

    public GetJournalNameIdsRequestHandler(SoarBeyondDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<HashSet<JournalNameId>> Handle(GetJournalNameIdsRequest request, CancellationToken cancellationToken)
    {
        var journalIdNames = _dbContext.Journals
            .Where(j => j.UserId == request.UserId)
            .Select(j => new JournalNameId(j.Name, j.Id))
            .ToHashSet();

        return Task.FromResult(journalIdNames);
    }
}