using MediatR;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Providers.Interfaces;

namespace SoarBeyond.Domain.Mediator.Moments;

public class CreateMomentRequest
    : IRequest<Moment>
{
    public int UserId { get; init; }
    public int JournalId { get; init; }
    public Moment Moment { get; init; }
}

public class CreateMomentRequestHandler
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