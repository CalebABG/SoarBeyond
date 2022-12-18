using MediatR;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Providers.Interfaces;

namespace SoarBeyond.Domain.Mediator.Moments;

public sealed record GetMomentsFromJournalRequest(int UserId, int JournalId)
    : IRequest<IEnumerable<Moment>>;

internal sealed class GetMomentsFromJournalRequestHandler
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