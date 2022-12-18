using MediatR;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Providers.Interfaces;

namespace SoarBeyond.Domain.Mediator.Notes;

public sealed record CreateNoteRequest(int UserId, int MomentId, Note Note)
    : IRequest<Note>;

internal sealed class CreateNoteRequestHandler
    : IRequestHandler<CreateNoteRequest, Note>
{
    private readonly INoteProvider _noteProvider;

    public CreateNoteRequestHandler(INoteProvider noteProvider)
    {
        _noteProvider = noteProvider;
    }

    public async Task<Note> Handle(CreateNoteRequest request, CancellationToken cancellationToken)
    {
        return await _noteProvider.CreateAsync(request);
    }
}