using MediatR;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Providers.Interfaces;

namespace SoarBeyond.Domain.Mediator.Journals;

public sealed record CreateJournalRequest(int UserId, Journal Journal)
    : IRequest<Journal>;

internal sealed class CreateJournalRequestHandler
    : IRequestHandler<CreateJournalRequest, Journal>
{
    private readonly IJournalProvider _journalProvider;

    public CreateJournalRequestHandler(IJournalProvider journalProvider)
    {
        _journalProvider = journalProvider;
    }

    public async Task<Journal> Handle(CreateJournalRequest request, CancellationToken cancellationToken)
    {
        return await _journalProvider.CreateAsync(request);
    }
}