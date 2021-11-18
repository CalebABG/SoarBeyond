using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SoarBeyond.Data;
using SoarBeyond.Data.Entities;
using SoarBeyond.Domain.MediatR.JournalEntries;
using SoarBeyond.Domain.Providers.Interfaces;
using SoarBeyond.Shared.Dto;

namespace SoarBeyond.Domain.Providers;

public class DbJournalEntryProvider : IJournalEntryProvider
{
    private readonly IMapper _mapper;
    private readonly IDbContextFactory<SoarBeyondDbContext> _dbContextFactory;

    public DbJournalEntryProvider(
        IDbContextFactory<SoarBeyondDbContext> dbContextFactory,
        IMapper mapper)
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }

    public async Task<JournalEntry> CreateAsync(CreateJournalEntryRequest request)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var dbJournalEntry = await dbContext.JournalEntries
            .Include(je => je.Thoughts)
            .FirstOrDefaultAsync(je => je.Id == request.JournalEntry.Id
                                       && je.JournalId == request.JournalId
                                       && je.UserId == request.UserId);

        if (dbJournalEntry is not null)
            return null;

        var journalEntry = _mapper.Map<JournalEntry, JournalEntryEntity>(request.JournalEntry);
        journalEntry.UserId = request.UserId;
        journalEntry.JournalId = request.JournalId;

        // Set each Thought UserId
        var thoughts = journalEntry.Thoughts;
        foreach (var t in thoughts)
            t.UserId = request.UserId;

        var addedJournalEntry = dbContext.JournalEntries.Add(journalEntry);
        await dbContext.SaveChangesAsync();

        var dto = _mapper.Map<JournalEntryEntity, JournalEntry>(addedJournalEntry.Entity);
        return dto;
    }

    public async Task<bool> DeleteAsync(DeleteJournalEntryRequest request)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var dbJournalEntry = await dbContext.JournalEntries
            .FirstOrDefaultAsync(je => je.JournalId == request.JournalId
                                       && je.Id == request.JournalEntryId
                                       && je.UserId == request.UserId);

        if (dbJournalEntry is null)
            return false;

        dbContext.JournalEntries.Remove(dbJournalEntry);

        var deleted = await dbContext.SaveChangesAsync() > 0;
        return deleted;
    }

    public async Task<JournalEntry> GetAsync(GetJournalEntryRequest request)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var dbJournalEntry = await dbContext.JournalEntries
            .Include(je => je.Thoughts)
            .FirstOrDefaultAsync(je => je.Id == request.JournalEntryId
                                       && je.UserId == request.UserId);

        if (dbJournalEntry is null)
            return null;

        var dto = _mapper.Map<JournalEntryEntity, JournalEntry>(dbJournalEntry);
        return dto;
    }

    public async Task<JournalEntry> UpdateAsync(UpdateJournalEntryRequest request)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var dbJournalEntry = await dbContext.JournalEntries
            .Include(je => je.Thoughts)
            .FirstOrDefaultAsync(je => je.UserId == request.UserId &&
                                       je.Id == request.JournalEntryId);

        if (dbJournalEntry is null)
            return null;

        _mapper.Map(request.JournalEntry, dbJournalEntry);
        await dbContext.SaveChangesAsync();

        var dto = _mapper.Map<JournalEntryEntity, JournalEntry>(dbJournalEntry);
        return dto;
    }

    public async Task<IEnumerable<JournalEntry>> GetByJournalIdAsync(GetJournalEntriesByJournalIdRequest request)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var journalEntries = dbContext.JournalEntries
            .Include(je => je.Thoughts)
            .Where(je => je.JournalId == request.JournalId
                         && je.UserId == request.UserId);

        var entries = await journalEntries
            .ProjectTo<JournalEntry>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return entries;
    }

    public async Task<IEnumerable<JournalEntry>> GetAllAsync(GetAllJournalEntriesRequest request)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var journalEntries = dbContext.JournalEntries
            .Include(je => je.Thoughts)
            .Where(je => je.UserId == request.UserId);

        var entries = await journalEntries
            .ProjectTo<JournalEntry>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return entries;
    }
}