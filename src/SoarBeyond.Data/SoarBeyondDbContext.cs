using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SoarBeyond.Data.Entities;

namespace SoarBeyond.Data;

// Todo: Place AsNoTracking after props for inclusion where needed
// Todo: Postgres DateTime/DateTimeOffset DefaultValueGeneration isn't currently supported/provided
public class SoarBeyondDbContext 
    : IdentityDbContext<UserEntity, IdentityRole<int>, int>
{
    public DbSet<JournalEntity> Journals { get; set; }
    public DbSet<MomentEntity> Moments { get; set; }
    public DbSet<NoteEntity> Notes { get; set; }
    public DbSet<ReflectionEntity> Reflections { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }

    public SoarBeyondDbContext(DbContextOptions<SoarBeyondDbContext> options)
        : base(options)
    {
        Console.WriteLine($"{ContextId} context created");
    }

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

    public override async ValueTask DisposeAsync()
    {
        Console.WriteLine($"{ContextId} context disposed async");
        await base.DisposeAsync();
    }
}