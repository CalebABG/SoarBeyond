﻿using MediatR;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Providers.Interfaces;

namespace SoarBeyond.Domain.Mediator.Journals;

public class CreateJournalRequest
    : IRequest<Journal>
{
    public int UserId { get; init; }
    public Journal Journal { get; init; }
}

public class CreateJournalRequestHandler
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