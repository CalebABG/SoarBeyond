using MediatR;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Providers.Interfaces;

namespace SoarBeyond.Domain.Mediator.Moments;

public class GetMomentsFromJournalRequest
    : IRequest<IEnumerable<Moment>>
{
    public int UserId { get; init; }
    public int JournalId { get; init; }
}

public class GetMomentsFromJournalRequestHandler
    : IRequestHandler<GetMomentsFromJournalRequest, IEnumerable<Moment>>
{
    private readonly IMomentProvider _momentProvider;

    public GetMomentsFromJournalRequestHandler(IMomentProvider momentProvider)
    {
        _momentProvider = momentProvider;
    }

    public async Task<IEnumerable<Moment>> Handle(GetMomentsFromJournalRequest request, CancellationToken cancellationToken)
    {
        return await _momentProvider.GetByJournalIdAsync(request);
    }
}