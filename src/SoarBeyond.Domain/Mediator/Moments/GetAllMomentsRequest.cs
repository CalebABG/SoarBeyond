using MediatR;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Providers.Interfaces;

namespace SoarBeyond.Domain.Mediator.Moments;

public sealed record GetAllMomentsRequest(int UserId)
    : IRequest<IEnumerable<Moment>>;

internal sealed class GetAllMomentsRequestHandler
    : IRequestHandler<GetAllMomentsRequest, IEnumerable<Moment>>
{
    private readonly IMomentProvider _momentProvider;

    public GetAllMomentsRequestHandler(IMomentProvider momentProvider)
    {
        _momentProvider = momentProvider;
    }

    public async Task<IEnumerable<Moment>> Handle(GetAllMomentsRequest request, CancellationToken cancellationToken)
    {
        return await _momentProvider.GetAllAsync(request);
    }
}