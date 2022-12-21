using MediatR;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Providers.Interfaces;

namespace SoarBeyond.Domain.Requests.Moments;

public sealed record UpdateMomentRequest(int UserId, Moment Moment)
    : IRequest<Moment>;

internal sealed class UpdateMomentRequestHandler
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