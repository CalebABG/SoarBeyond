using MediatR;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Providers.Interfaces;

namespace SoarBeyond.Domain.Requests.Moments;

public sealed record CreateMomentRequest(int UserId, int JournalId, Moment Moment)
    : IRequest<Moment>;

internal sealed class CreateMomentRequestHandler
    : IRequestHandler<CreateMomentRequest, Moment>
{
    private readonly IMomentProvider _momentProvider;

    public CreateMomentRequestHandler(IMomentProvider momentProvider)
    {
        _momentProvider = momentProvider;
    }

    public async Task<Moment> Handle(CreateMomentRequest request, CancellationToken cancellationToken)
    {
        return await _momentProvider.CreateAsync(request);
    }
}