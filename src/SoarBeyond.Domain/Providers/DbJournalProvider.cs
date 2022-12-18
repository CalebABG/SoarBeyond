using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SoarBeyond.Data;
using SoarBeyond.Data.Entities;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Mediator.Journals;
using SoarBeyond.Domain.Providers.Interfaces;
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
            .Include(j => j.Moments)
            .ThenInclude(m => m.Notes)
            .AsNoTracking()
            .FirstOrDefaultAsync(j => j.UserId == request.UserId &&
                                      j.Id == request.Journal.Id);

        if (dbJournal is not null)
            return null;

        JournalEntity journal = _mapper.Map<Journal, JournalEntity>(request.Journal);
        journal.UserId = request.UserId;

        var addedJournal = context.Journals.Add(journal);
        await context.SaveChangesAsync();

        return _mapper.Map<JournalEntity, Journal>(addedJournal.Entity);
    }

    public async Task<Journal> UpdateAsync(UpdateJournalRequest request)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        // Todo: Place AsNoTracking after props for inclusion where needed
        var journal = await context.Journals
            .Include(j => j.Moments)
            .ThenInclude(m => m.Notes)
            .AsNoTracking()
            .FirstOrDefaultAsync(j => j.UserId == request.UserId &&
                                      j.Id == request.JournalId);

        if (journal is null)
            return null;

        _mapper.Map(request.Journal, journal);
        var updatedEntry = context.Journals.Update(journal);

        await context.SaveChangesAsync();

        return _mapper.Map<JournalEntity, Journal>(updatedEntry.Entity);
    }

    public async Task<bool> DeleteAsync(DeleteJournalRequest request)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var journal = await context.Journals
            .AsNoTracking()
            .FirstOrDefaultAsync(j => j.UserId == request.UserId &&
                                      j.Id == request.JournalId);

        if (journal is null)
            return false;

        context.Journals.Remove(journal);

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<Journal> GetAsync(GetJournalRequest request)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var journal = await context.Journals
            .Include(j => j.Moments)
            .ThenInclude(je => je.Notes)
            .AsNoTracking()
            .FirstOrDefaultAsync(j => j.UserId == request.UserId &&
                                      j.Id == request.JournalId);

        if (journal is null)
            return null;

        return _mapper.Map<JournalEntity, Journal>(journal);
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
            .Include(j => j.Moments)
            .ThenInclude(m => m.Notes)
            .AsNoTracking()
            .Where(j => j.UserId == request.UserId);

        return await journals.ProjectTo<Journal>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<IEnumerable<Journal>> GetFavoritesAsync(GetFavoriteJournalsRequest request)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var journals = context.Journals
            .Include(j => j.Moments)
            .ThenInclude(m => m.Notes)
            .AsNoTracking()
            .Where(j => j.UserId == request.UserId && j.Favored);

        return await journals.ProjectTo<Journal>(_mapper.ConfigurationProvider).ToListAsync();
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

        journal.Favored = request.Favored;
        context.Journals.Update(journal);

        return await context.SaveChangesAsync() > 0;
    }
}