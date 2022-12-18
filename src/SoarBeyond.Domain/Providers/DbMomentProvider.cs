using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SoarBeyond.Data;
using SoarBeyond.Data.Entities;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Mediator.Moments;
using SoarBeyond.Domain.Providers.Interfaces;

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
            .FirstOrDefaultAsync(m => m.Id == request.Moment.Id
                                       && m.JournalId == request.JournalId
                                       && m.Journal.UserId == request.UserId);

        if (dbMoment is not null)
            return null;

        var moment = _mapper.Map<Moment, MomentEntity>(request.Moment);
        moment.JournalId = request.JournalId;

        var addedMoment = context.Moments.Add(moment);
        await context.SaveChangesAsync();

        return _mapper.Map<MomentEntity, Moment>(addedMoment.Entity);
    }

    public async Task<bool> DeleteAsync(DeleteMomentRequest request)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var moment = await context.Moments
            .Include(m => m.Journal)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.JournalId == request.JournalId && 
                                      m.Id == request.MomentId && 
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
            .FirstOrDefaultAsync(m => m.Id == request.MomentId
                                      && m.Journal.UserId == request.UserId);

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
            .FirstOrDefaultAsync(m => m.Journal.UserId == request.UserId &&
                                       m.Id == request.MomentId);

        if (moment is null)
            return null;

        _mapper.Map(request.Moment, moment);
        var updatedEntry = context.Moments.Update(moment);
        
        await context.SaveChangesAsync();

        var dto = _mapper.Map<MomentEntity, Moment>(updatedEntry.Entity);
        return dto;
    }

    public async Task<IEnumerable<Moment>> GetByJournalIdAsync(GetMomentsFromJournalRequest request)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var moments = context.Moments
            .Include(m => m.Journal)
            .Include(m => m.Notes)
            .AsNoTracking()
            .Where(m => m.JournalId == request.JournalId && 
                         m.Journal.UserId == request.UserId);

        return await moments.ProjectTo<Moment>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<IEnumerable<Moment>> GetAllAsync(GetAllMomentsRequest request)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var moments = context.Moments
            .Include(m => m.Journal)
            .Include(m => m.Notes)
            .AsNoTracking()
            .Where(m => m.Journal.UserId == request.UserId);

        return await moments.ProjectTo<Moment>(_mapper.ConfigurationProvider).ToListAsync();
    }
}