using MediatR;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Providers.Interfaces;

namespace SoarBeyond.Domain.Mediator.Moments;

public class UpdateMomentRequest
    : IRequest<Moment>
{
    public int UserId { get; init; }
    public int MomentId { get; init; }
    public Moment Moment { get; init; }
}

public class UpdateMomentRequestHandler
    : IRequestHandler<UpdateMomentRequest, Moment>
{
    private readonly IMomentProvider _momentProvider;

    public UpdateMomentRequestHandler(IMomentProvider momentProvider)
    {
        _momentProvider = momentProvider;
    }

    public async Task<Moment> Handle(UpdateMomentRequest request, CancellationToken cancellationToken)
    {
        return await _momentProvider.UpdateAsync(request);
    }
}