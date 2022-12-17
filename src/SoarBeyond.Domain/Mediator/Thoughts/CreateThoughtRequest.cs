using MediatR;
using SoarBeyond.Domain.Providers.Interfaces;
using SoarBeyond.Shared.Dto;

namespace SoarBeyond.Domain.Mediator.Thoughts;

public class CreateThoughtRequest
    : IRequest<Thought>
{
    public int UserId { get; init; }
    public int JournalEntryId { get; init; }
    public Thought Thought { get; init; }
}

public class CreateThoughtRequestHandler
    : IRequestHandler<CreateThoughtRequest, Thought>
{
    private readonly IThoughtProvider _thoughtProvider;

    public CreateThoughtRequestHandler(IThoughtProvider thoughtProvider)
    {
        _thoughtProvider = thoughtProvider;
    }

    public async Task<Thought> Handle(CreateThoughtRequest request, CancellationToken cancellationToken)
    {
        return await _thoughtProvider.CreateAsync(request);
    }
}