using MediatR;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Providers.Interfaces;

namespace SoarBeyond.Domain.Mediator.Journals;

public sealed record UpdateJournalRequest(int UserId, Journal Journal)
    : IRequest<Journal>;

internal sealed class UpdateJournalRequestHandler
    : IRequestHandler<UpdateJournalRequest, Journal>
{
    private readonly IJournalProvider _journalProvider;

    public UpdateJournalRequestHandler(IJournalProvider journalProvider)
    {
        _journalProvider = journalProvider;
    }

    public async Task<Journal> Handle(UpdateJournalRequest request, CancellationToken cancellationToken)
    {
        return await _journalProvider.UpdateAsync(request);
    }
}