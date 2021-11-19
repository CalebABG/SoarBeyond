using Microsoft.EntityFrameworkCore;

namespace SoarBeyond.Data.Seed;

public interface ISeedSource<in TDbContext>
    where TDbContext : DbContext
{
    Task Seed(TDbContext dbContext);
}