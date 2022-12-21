using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SoarBeyond.Data;
using SoarBeyond.Data.Entities;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Providers.Interfaces;
using SoarBeyond.Domain.Requests.Moments;

namespace SoarBeyond.Domain.Providers;

public class DbMomentProvider : IMomentProvider
{
    private readonly IMapper _mapper;
    private readonly IDbContextFactory<SoarBeyondDbContext> _contextFactory;

    public DbMomentProvider(
        IMapper mapper,
        IDbContextFactory<SoarBeyondDbContext> contextFactory)
    {
        _mapper = mapper;
        _contextFactory = contextFactory;
    }

    public async Task<Moment> CreateAsync(CreateMomentRequest request)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var dbMoment = await context.Moments
            .Include(m => m.Journal)
            .Include(m => m.Notes)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == request.Moment.Id && 
                                      m.JournalId == request.JournalId && 
                                      m.Journal.UserId == request.UserId);

        if (dbMoment is not null)
            return null;

        var mappedMoment = _mapper.Map<Moment, MomentEntity>(request.Moment);
        mappedMoment.JournalId = request.JournalId;

        var addedEntry = context.Moments.Add(mappedMoment);
        await context.SaveChangesAsync();

        return _mapper.Map<MomentEntity, Moment>(addedEntry.Entity);
    }

    public async Task<bool> DeleteAsync(DeleteMomentRequest request)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var moment = await context.Moments
            .Include(m => m.Journal)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == request.MomentId && 
                                      m.Journal.UserId == request.UserId);

        if (moment is null)
            return false;

        context.Moments.Remove(moment);

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<Moment> GetAsync(GetMomentRequest request)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var moment = await context.Moments
            .Include(m => m.Journal)
            .Include(m => m.Notes)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == request.MomentId && 
                                      m.Journal.UserId == request.UserId);

        if (moment is null)
            return null;

        return _mapper.Map<MomentEntity, Moment>(moment);
    }

    public async Task<Moment> UpdateAsync(UpdateMomentRequest request)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var moment = await context.Moments
            .Include(m => m.Journal)
            .Include(m => m.Notes)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == request.Moment.Id && 
                                      m.Journal.UserId == request.UserId);

        if (moment is null)
            return null;

        _mapper.Map(request.Moment, moment);
        moment.UpdatedDate = DateTimeOffset.UtcNow;

        var updatedEntry = context.Moments.Update(moment);
        await context.SaveChangesAsync();

        return _mapper.Map<MomentEntity, Moment>(updatedEntry.Entity);
    }

    public async Task<IEnumerable<Moment>> GetByJournalIdAsync(GetMomentsFromJournalRequest request)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var moments = context.Moments
            .Include(m => m.Journal)
            .Include(m => m.Notes)
            .AsNoTracking()
            .Where(m => m.JournalId == request.JournalId && 
                        m.Journal.UserId == request.UserId)
            .ProjectTo<Moment>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return await moments;
    }

    public async Task<IEnumerable<Moment>> GetAllAsync(GetAllMomentsRequest request)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var moments = context.Moments
            .Include(m => m.Journal)
            .Include(m => m.Notes)
            .AsNoTracking()
            .Where(m => m.Journal.UserId == request.UserId)
            .ProjectTo<Moment>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return await moments;
    }
}