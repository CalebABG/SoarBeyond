using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SoarBeyond.Data;
using SoarBeyond.Data.Entities;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Providers.Interfaces;
using SoarBeyond.Domain.Requests.Notes;

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
            .FirstOrDefaultAsync(n => n.Id == request.Note.Id && 
                                      n.MomentId == request.MomentId && 
                                      n.Moment.Journal.UserId == request.UserId);

        if (dbNote is not null)
            return null;

        var mappedNote = _mapper.Map<Note, NoteEntity>(request.Note);

        var addedEntry = context.Notes.Add(mappedNote);
        await context.SaveChangesAsync();

        return _mapper.Map<NoteEntity, Note>(addedEntry.Entity);
    }
}