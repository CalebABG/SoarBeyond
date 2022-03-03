using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SoarBeyond.Data;
using SoarBeyond.Data.Entities;
using SoarBeyond.Domain.MediatR.Journals;
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
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var dbJournal = await dbContext.Journals
            .Include(j => j.JournalEntries)
            .ThenInclude(je => je.Thoughts)
            .FirstOrDefaultAsync(j => j.UserId == request.UserId &&
                                      j.Id == request.Journal.Id);

        if (dbJournal is not null)
            return null;

        JournalEntity journal = _mapper.Map<Journal, JournalEntity>(request.Journal);
        journal.UserId = request.UserId;

        var addedJournal = dbContext.Journals.Add(journal);
        await dbContext.SaveChangesAsync();

        var dto = _mapper.Map<JournalEntity, Journal>(addedJournal.Entity);
        return dto;
    }

    public async Task<Journal> UpdateAsync(UpdateJournalRequest request)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var dbJournal = await dbContext.Journals
            .Include(j => j.JournalEntries)
            .ThenInclude(je => je.Thoughts)
            .FirstOrDefaultAsync(j => j.UserId == request.UserId &&
                                      j.Id == request.JournalId);

        if (dbJournal is null)
            return null;

        _mapper.Map(request.Journal, dbJournal);
        await dbContext.SaveChangesAsync();

        var dto = _mapper.Map<JournalEntity, Journal>(dbJournal);
        return dto;
    }

    public async Task<bool> DeleteAsync(DeleteJournalRequest request)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var dbJournal = await dbContext.Journals
            .FirstOrDefaultAsync(j => j.UserId == request.UserId &&
                                      j.Id == request.JournalId);

        if (dbJournal is null)
            return false;

        dbContext.Journals.Remove(dbJournal);

        var deleted = await dbContext.SaveChangesAsync() > 0;
        return deleted;
    }

    public async Task<Journal> GetAsync(GetJournalRequest request)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var dbJournal = await dbContext.Journals
            .Include(j => j.JournalEntries)
            .ThenInclude(je => je.Thoughts)
            .FirstOrDefaultAsync(j => j.UserId == request.UserId &&
                                      j.Id == request.JournalId);

        if (dbJournal is null)
            return null;

        var dto = _mapper.Map<JournalEntity, Journal>(dbJournal);
        return dto;
    }

    public async Task<HashSet<JournalNameId>> GetNameIdsAsync(GetJournalNameIdsRequest request)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var journalIdNames = dbContext.Journals
            .Where(j => j.UserId == request.UserId)
            .Select(j => new JournalNameId(j.Name, j.Id))
            .ToHashSet();

        return await Task.FromResult(journalIdNames);
    }

    public async Task<IEnumerable<Journal>> GetAllAsync(GetAllJournalsRequest request)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var journals = dbContext.Journals
            .Include(j => j.JournalEntries)
            .ThenInclude(journalEntry => journalEntry.Thoughts)
            .Where(j => j.UserId == request.UserId);

        var dtos = await journals
            .ProjectTo<Journal>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return dtos;
    }
}