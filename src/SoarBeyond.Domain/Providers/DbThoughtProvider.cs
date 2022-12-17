using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SoarBeyond.Data;
using SoarBeyond.Data.Entities;
using SoarBeyond.Domain.Mediator.Thoughts;
using SoarBeyond.Domain.Providers.Interfaces;
using SoarBeyond.Shared.Dto;

namespace SoarBeyond.Domain.Providers;

public class DbThoughtProvider : IThoughtProvider
{
    private readonly IMapper _mapper;
    private readonly IDbContextFactory<SoarBeyondDbContext> _dbContextFactory;

    public DbThoughtProvider(
        IDbContextFactory<SoarBeyondDbContext> dbContextFactory,
        IMapper mapper)
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }

    public async Task<Thought> CreateAsync(CreateThoughtRequest request)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var dbEntry = await dbContext.Thoughts
            .FirstOrDefaultAsync(t => t.Id == request.Thought.Id
                                      && t.JournalEntryId == request.JournalEntryId
                                      && t.UserId == request.UserId);

        if (dbEntry is not null) return null;

        ThoughtEntity thought = _mapper.Map<Thought, ThoughtEntity>(request.Thought);
        thought.UserId = request.UserId;

        var addedThought = dbContext.Thoughts.Add(thought);
        await dbContext.SaveChangesAsync();

        var dto = _mapper.Map<ThoughtEntity, Thought>(addedThought.Entity);
        return dto;
    }
}