using MediatR;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Providers.Interfaces;

namespace SoarBeyond.Domain.Mediator.Moments;

public sealed record GetMomentRequest(int UserId, int MomentId)
    : IRequest<Moment>;

internal sealed class GetMomentRequestHandler
    : IRequestHandler<GetMomentRequest, Moment>
{
    private readonly IMomentProvider _momentProvider;

    public GetMomentRequestHandler(IMomentProvider momentProvider)
    {
        _momentProvider = momentProvider;
    }

    public async Task<Moment> Handle(GetMomentRequest request, CancellationToken cancellationToken)
    {
        return await _momentProvider.GetAsync(request);
    }
}