using MediatR;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Providers.Interfaces;

namespace SoarBeyond.Domain.Mediator.Moments;

public class GetMomentRequest
    : IRequest<Moment>
{
    public int UserId { get; init; }
    public int MomentId { get; init; }
}

public class GetMomentRequestHandler
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