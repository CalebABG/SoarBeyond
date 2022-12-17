using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SoarBeyond.Data;
using SoarBeyond.Data.Entities;
using SoarBeyond.Domain.Mediator.Journals;
using SoarBeyond.Domain.Providers.Interfaces;
using SoarBeyond.Shared.Dto;
using SoarBeyond.Shared.Poco;

namespace SoarBeyond.Domain.Providers;

public class DbJournalProvider : IJournalProvider
{
    private readonly IMapper _mapper;
    private readonly IDbContextFactory<SoarBeyondDbContext> _dbContextFactory;

    public DbJournalProvider(
        IDbContextFactory<SoarBeyondDbContext> dbContextFactory,
        IMapper mapper)
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }

    public async Task<Journal> CreateAsync(CreateJournalRequest request)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var dbJournal = await context.Journals
            .Include(j => j.JournalEntries)
            .ThenInclude(je => je.Thoughts)
            .FirstOrDefaultAsync(j => j.UserId == request.UserId &&
                                      j.Id == request.Journal.Id);

        if (dbJournal is not null)
            return null;

        JournalEntity journal = _mapper.Map<Journal, JournalEntity>(request.Journal);
        journal.UserId = request.UserId;

        var addedJournal = context.Journals.Add(journal);
        await context.SaveChangesAsync();

        var dto = _mapper.Map<JournalEntity, Journal>(addedJournal.Entity);
        return dto;
    }

    public async Task<Journal> UpdateAsync(UpdateJournalRequest request)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var journal = await context.Journals
            .Include(j => j.JournalEntries)
            .ThenInclude(je => je.Thoughts)
            .FirstOrDefaultAsync(j => j.UserId == request.UserId &&
                                      j.Id == request.JournalId);

        if (journal is null)
            return null;

        _mapper.Map(request.Journal, journal);
        await context.SaveChangesAsync();

        return _mapper.Map<JournalEntity, Journal>(journal);
    }

    public async Task<bool> DeleteAsync(DeleteJournalRequest request)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var journal = await context.Journals
            .FirstOrDefaultAsync(j => j.UserId == request.UserId &&
                                      j.Id == request.JournalId);

        if (journal is null)
            return false;

        context.Journals.Remove(journal);

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<Journal> GetAsync(GetJournalRequest request)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var journal = await context.Journals
            .Include(j => j.JournalEntries)
            .ThenInclude(je => je.Thoughts)
            .FirstOrDefaultAsync(j => j.UserId == request.UserId &&
                                      j.Id == request.JournalId);

        if (journal is null)
            return null;

        return _mapper.Map<JournalEntity, Journal>(journal);
    }

    public async Task<HashSet<JournalNameId>> GetNameIdsAsync(GetJournalNameIdsRequest request)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var journalIdNames = context.Journals
            .Where(j => j.UserId == request.UserId)
            .Select(j => new JournalNameId(j.Name, j.Id))
            .ToHashSet();

        return await Task.FromResult(journalIdNames);
    }

    // Todo: Return IEnumerable (client side filtering) or IQueryable (server side filtering) where possible
    // Todo: Use AsNoTracking where possible (when readonly data), especially using DTOs
    public async Task<IEnumerable<Journal>> GetAllAsync(GetAllJournalsRequest request)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var journals = context.Journals
            .Include(j => j.JournalEntries)
            .ThenInclude(journalEntry => journalEntry.Thoughts)
            .Where(j => j.UserId == request.UserId);

        return journals.ProjectTo<Journal>(_mapper.ConfigurationProvider);
    }
}