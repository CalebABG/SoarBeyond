using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SoarBeyond.Data.Entities;

namespace SoarBeyond.Data;

public class SoarBeyondDbContext : IdentityDbContext<SoarBeyondUserEntity, IdentityRole<int>, int>
{
    public SoarBeyondDbContext(DbContextOptions<SoarBeyondDbContext> options)
        : base(options)
    {
        Console.WriteLine($"{ContextId} context created");
    }

    public DbSet<JournalEntity> Journals { get; set; }
    public DbSet<JournalEntryEntity> JournalEntries { get; set; }
    public DbSet<ThoughtEntity> Thoughts { get; set; }
    public DbSet<ReflectionEntity> Reflections { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(SoarBeyondDbContext).Assembly);
    }

    public override void Dispose()
    {
        Console.WriteLine($"{ContextId} context disposed");
        base.Dispose();
    }

    public override ValueTask DisposeAsync()
    {
        Console.WriteLine($"{ContextId} context disposed async");
        return base.DisposeAsync();
    }
}