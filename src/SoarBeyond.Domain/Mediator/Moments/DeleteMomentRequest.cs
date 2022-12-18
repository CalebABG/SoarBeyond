using MediatR;
using SoarBeyond.Domain.Providers.Interfaces;

namespace SoarBeyond.Domain.Mediator.Moments;

public sealed record DeleteMomentRequest(int UserId, int MomentId)
    : IRequest<bool>;

internal sealed class DeleteMomentRequestHandler
    : IRequestHandler<DeleteMomentRequest, bool>
{
    private readonly IMomentProvider _momentProvider;

    public DeleteMomentRequestHandler(IMomentProvider momentProvider)
    {
        _momentProvider = momentProvider;
    }

    public async Task<bool> Handle(DeleteMomentRequest request, CancellationToken cancellationToken)
    {
        return await _momentProvider.DeleteAsync(request);
    }
}