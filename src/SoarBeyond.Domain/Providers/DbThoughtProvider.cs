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
    private readonly IDbContextFactory<SoarBeyondDbContext> _contextFactory;

    public DbThoughtProvider(
        IDbContextFactory<SoarBeyondDbContext> contextFactory,
        IMapper mapper)
    {
        _contextFactory = contextFactory;
        _mapper = mapper;
    }

    public async Task<Thought> CreateAsync(CreateThoughtRequest request)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var entry = await context.Thoughts
            .FirstOrDefaultAsync(t => t.Id == request.Thought.Id
                                      && t.JournalEntryId == request.JournalEntryId
                                      && t.UserId == request.UserId);

        if (entry is not null)
            return null;

        ThoughtEntity thought = _mapper.Map<Thought, ThoughtEntity>(request.Thought);
        thought.UserId = request.UserId;

        var addedThought = context.Thoughts.Add(thought);
        await context.SaveChangesAsync();

        var dto = _mapper.Map<ThoughtEntity, Thought>(addedThought.Entity);
        return dto;
    }
}