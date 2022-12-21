using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Requests.Notes;

namespace SoarBeyond.Domain.Providers.Interfaces;

public interface INoteProvider
{
    Task<Note> CreateAsync(CreateNoteRequest request);
}