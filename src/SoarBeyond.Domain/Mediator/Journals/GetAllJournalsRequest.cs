using MediatR;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Providers.Interfaces;

namespace SoarBeyond.Domain.Mediator.Journals;

public class GetAllJournalsRequest
    : IRequest<IEnumerable<Journal>>
{
    public int UserId { get; init; }
}

public class GetAllJournalsRequestHandler
    : IRequestHandler<GetAllJournalsRequest, IEnumerable<Journal>>
{
    private readonly IJournalProvider _journalProvider;

    public GetAllJournalsRequestHandler(IJournalProvider journalProvider)
    {
        _journalProvider = journalProvider;
    }

    public async Task<IEnumerable<Journal>> Handle(GetAllJournalsRequest request, CancellationToken cancellationToken)
    {
        return await _journalProvider.GetAllAsync(request);
    }
}