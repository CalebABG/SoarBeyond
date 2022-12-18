using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SoarBeyond.Data;
using SoarBeyond.Data.Entities;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Mediator.Notes;
using SoarBeyond.Domain.Providers.Interfaces;

namespace SoarBeyond.Domain.Providers;

public class DbNoteProvider : INoteProvider
{
    private readonly IMapper _mapper;
    private readonly IDbContextFactory<SoarBeyondDbContext> _contextFactory;

    public DbNoteProvider(
        IMapper mapper,
        IDbContextFactory<SoarBeyondDbContext> contextFactory)
    {
        _mapper = mapper;
        _contextFactory = contextFactory;
    }

    public async Task<Note> CreateAsync(CreateNoteRequest request)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var dbNote = await context.Notes
            .Include(n => n.Moment)
            .ThenInclude(m => m.Journal)
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == request.Note.Id
                                      && t.MomentId == request.MomentId
                                      && t.Moment.Journal.UserId == request.UserId);

        if (dbNote is not null)
            return null;

        NoteEntity note = _mapper.Map<Note, NoteEntity>(request.Note);

        var addedNote = context.Notes.Add(note);
        await context.SaveChangesAsync();

        return _mapper.Map<NoteEntity, Note>(addedNote.Entity);
    }
}