using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Mediator.Notes;

namespace SoarBeyond.Domain.Providers.Interfaces;

public interface INoteProvider
{
    Task<Note> CreateAsync(CreateNoteRequest request);
}