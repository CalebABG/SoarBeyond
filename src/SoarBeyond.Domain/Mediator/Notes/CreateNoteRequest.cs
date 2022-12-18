using MediatR;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Providers.Interfaces;

namespace SoarBeyond.Domain.Mediator.Notes;

public class CreateNoteRequest
    : IRequest<Note>
{
    public int UserId { get; init; }
    public int MomentId { get; init; }
    public Note Note { get; init; }
}

public class CreateNoteRequestHandler
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