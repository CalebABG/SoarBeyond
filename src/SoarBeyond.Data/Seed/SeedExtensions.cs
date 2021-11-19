using Microsoft.EntityFrameworkCore;

namespace SoarBeyond.Data.Seed;

public static class SeedExtensions
{
    public static async Task SeedDatabase<TDbContext>(this TDbContext context)
        where TDbContext : DbContext
    {
        var seedSources = typeof(SeedExtensions).Assembly.ExportedTypes
            .Where(type => typeof(ISeedSource<TDbContext>).IsAssignableFrom(type) &&
                           !type.IsInterface &&
                           !type.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<ISeedSource<TDbContext>>()
            .ToList();

        var seedTasks = new List<Task>();
        for (var i = 0; i < seedSources.Count; ++i)
            seedTasks.Add(seedSources[i].Seed(context));

        await Task.WhenAll(seedTasks);
    }
}