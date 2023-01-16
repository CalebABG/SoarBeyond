using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SoarBeyond.Data;
using SoarBeyond.Data.Entities;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Providers.Interfaces;
using SoarBeyond.Domain.Requests.Journals;
using SoarBeyond.Shared.Poco;

namespace SoarBeyond.Domain.Providers;

public class DbJournalProvider : IJournalProvider
{
    private readonly IMapper _mapper;
    private readonly IDbContextFactory<SoarBeyondDbContext> _contextFactory;

    public DbJournalProvider(
        IMapper mapper,
        IDbContextFactory<SoarBeyondDbContext> contextFactory)
    {
        _mapper = mapper;
        _contextFactory = contextFactory;
    }

    public async Task<Journal> CreateAsync(CreateJournalRequest request)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var dbJournal = await context.Journals
            .AsNoTracking()
            .FirstOrDefaultAsync(j => j.UserId == request.UserId &&
                                      j.Id == request.Journal.Id);

        if (dbJournal is not null)
            return null;

        var mappedJournal = _mapper.Map<Journal, JournalEntity>(request.Journal);
        mappedJournal.UserId = request.UserId;

        var addedEntry = context.Journals.Add(mappedJournal);
        bool saved = await context.SaveChangesAsync() > 0;

        var addedJournalId = addedEntry.Entity.Id;

        // Todo: Better way to save querying db again?
        var newDbJournal = await context.Journals
            .Include(j => j.Category)
            .Include(j => j.Moments)
            .ThenInclude(m => m.Notes)
            .AsNoTracking()
            .FirstOrDefaultAsync(j => j.UserId == request.UserId &&
                                      j.Id == addedJournalId);

        return _mapper.Map<JournalEntity, Journal>(newDbJournal);
    }

    public async Task<Journal> UpdateAsync(UpdateJournalRequest request)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var dbJournal = await context.Journals
            .Include(j => j.Category)
            .Include(j => j.Moments)
            .ThenInclude(m => m.Notes)
            .AsNoTracking()
            .FirstOrDefaultAsync(j => j.Id == request.Journal.Id && 
                                      j.UserId == request.UserId);

        if (dbJournal is null)
            return null;

        _mapper.Map(request.Journal, dbJournal);

        // Todo: Find better solution (when setting a new Category)
        if (request.Journal.Category is null)
        {
            dbJournal.CategoryId = null;
            dbJournal.Category = null;
        }

        dbJournal.UpdatedDate = DateTimeOffset.UtcNow;
        var updatedEntry = context.Journals.Update(dbJournal);

        await context.SaveChangesAsync();

        return _mapper.Map<JournalEntity, Journal>(updatedEntry.Entity);
    }

    public async Task<bool> DeleteAsync(DeleteJournalRequest request)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var dbJournal = await context.Journals
            .FirstOrDefaultAsync(j => j.UserId == request.UserId &&
                                      j.Id == request.JournalId);

        if (dbJournal is null)
            return false;

        context.Journals.Remove(dbJournal);

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<Journal> GetAsync(GetJournalRequest request)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var dbJournal = await context.Journals
            .Include(j => j.Category)
            .Include(j => j.Moments)
            .ThenInclude(m => m.Notes)
            .AsNoTracking()
            .FirstOrDefaultAsync(j => j.UserId == request.UserId &&
                                      j.Id == request.JournalId);

        if (dbJournal is null)
            return null;

        return _mapper.Map<JournalEntity, Journal>(dbJournal);
    }

    public async Task<IEnumerable<JournalName>> GetNamesAsync(GetJournalNamesRequest request)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var names = context.Journals
            .AsNoTracking()
            .Where(j => j.UserId == request.UserId)
            .Select(j => new JournalName(j.Name, j.Id))
            .AsEnumerable();

        return await Task.FromResult(names);
    }

    public async Task<IEnumerable<Journal>> GetAllAsync(GetAllJournalsRequest request)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var journals = context.Journals
            .Include(j => j.Category)
            .Include(j => j.Moments)
            .ThenInclude(m => m.Notes)
            .AsNoTracking()
            .Where(j => j.UserId == request.UserId)
            .ProjectTo<Journal>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return await journals;
    }

    public async Task<IEnumerable<Journal>> GetFavoritesAsync(GetFavoriteJournalsRequest request)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var journals = context.Journals
            .Include(j => j.Category)
            .Include(j => j.Moments)
            .ThenInclude(m => m.Notes)
            .AsNoTracking()
            .Where(j => j.UserId == request.UserId && j.Favorited)
            .ProjectTo<Journal>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return await journals;
    }

    public async Task<bool> UpdateFavoriteStatusAsync(UpdateJournalFavoriteStatusRequest request)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var journal = await context.Journals
            .AsNoTracking()
            .FirstOrDefaultAsync(j => j.UserId == request.UserId &&
                                      j.Id == request.JournalId);

        if (journal is null)
            return false;

        journal.Favorited = request.Favorited;
        journal.UpdatedDate = DateTimeOffset.UtcNow;
        context.Journals.Update(journal);

        return await context.SaveChangesAsync() > 0;
    }
}