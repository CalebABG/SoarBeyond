using MediatR;
using SoarBeyond.Domain.Providers.Interfaces;

namespace SoarBeyond.Domain.Mediator.Moments;

public class DeleteMomentRequest : IRequest<bool>
{
    public int UserId { get; init; }
    public int JournalId { get; init; }
    public int MomentId { get; init; }
}

public class DeleteMomentRequestHandler
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