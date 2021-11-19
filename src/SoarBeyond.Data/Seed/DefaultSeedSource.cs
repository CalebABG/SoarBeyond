using Bogus;
using Bogus.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScottBrady91.AspNetCore.Identity;
using SoarBeyond.Data.Entities;

namespace SoarBeyond.Data.Seed;

public class DefaultSeedSource : ISeedSource<SoarBeyondDbContext>
{
    private const string AdminEmail = "admin@admin.com";
    private const string AdminPass = "Pass123!";
    private const string AdminRoleName = "admin";

    public async Task Seed(SoarBeyondDbContext dbContext)
    {
        if (!await dbContext.Users.AnyAsync())
        {
            var pwHasher = new BCryptPasswordHasher<SoarBeyondUserEntity>();

            var admin = new SoarBeyondUserEntity
            {
                Email = AdminEmail,
                NormalizedEmail = AdminEmail.ToUpper(),
                UserName = AdminEmail,
                NormalizedUserName = AdminEmail.ToUpper(),
                EmailConfirmed = true,
                PasswordHash = pwHasher.HashPassword(null, AdminPass),
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = true,
            };

            dbContext.Users.Add(admin);
            await dbContext.SaveChangesAsync();

            Console.WriteLine("ADDED SEED DATA: 'Users'");
        }

        if (!await dbContext.Roles.AnyAsync())
        {
            Console.WriteLine("ADDED SEED DATA: 'Roles'");
            dbContext.Roles.AddRange(new List<IdentityRole<int>>
            {
                new()
                {
                    Name = AdminRoleName,
                    NormalizedName = AdminRoleName.ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }
            });
            await dbContext.SaveChangesAsync();
        }

        if (!await dbContext.UserRoles.AnyAsync())
        {
            var adminUser = await GetAdminUser(dbContext);
            var adminUserRole = await GetAdminUserRole(dbContext);

            dbContext.UserRoles.AddRange(new List<IdentityUserRole<int>>
            {
                new()
                {
                    RoleId = adminUserRole.Id,
                    UserId = adminUser.Id,
                }
            });
            await dbContext.SaveChangesAsync();

            Console.WriteLine("ADDED SEED DATA: 'User Roles'");
        }

        if (!await dbContext.Journals.AnyAsync())
        {
            var adminUser = await GetAdminUser(dbContext);

            var journalsFaker = new Faker<JournalEntity>()
                .RuleFor(m => m.Name, f => f.Lorem.Sentence(5))
                .RuleFor(m => m.Description, f => f.Lorem.Sentences(3))
                .RuleFor(m => m.CreationDate, f => f.Date.Between(DateTime.UtcNow.AddMonths(-5), DateTime.UtcNow))
                .RuleFor(m => m.UserId, f => adminUser.Id);

            var journalFakerData = journalsFaker.GenerateBetween(46, 68);

            dbContext.Journals.AddRange(journalFakerData);
            await dbContext.SaveChangesAsync();

            Console.WriteLine("ADDED SEED DATA: 'Journals'");
        }

        if (!await dbContext.JournalEntries.AnyAsync())
        {
            var users = await dbContext.Users.ToListAsync();
            var journals = await dbContext.Journals.ToListAsync();

            var usersCount = users.Count;
            var journalsCount = journals.Count;

            var journalEntryUserIndex = 0;
            var journalEntryJournalIndex = 0;

            var journalEntriesFaker = new Faker<JournalEntryEntity>()
                .RuleFor(m => m.Title, f => f.Lorem.Sentence(5))
                .RuleFor(m => m.Content, f => f.Lorem.Sentences(3))
                .RuleFor(m => m.CreationDate, f => f.Date.Between(DateTime.UtcNow.AddMonths(-5), DateTime.UtcNow))
                .RuleFor(m => m.UserId, f => users[DataIndex(ref journalEntryUserIndex, usersCount)].Id)
                .RuleFor(m => m.JournalId, f => journals[DataIndex(ref journalEntryJournalIndex, journalsCount)].Id);

            var journalEntriesFakerData = journalEntriesFaker.GenerateBetween(95, 145);

            dbContext.JournalEntries.AddRange(journalEntriesFakerData);
            await dbContext.SaveChangesAsync();

            Console.WriteLine("ADDED SEED DATA: 'JournalEntries'");
        }

        if (!await dbContext.Thoughts.AnyAsync())
        {
            var users = await dbContext.Users.ToListAsync();
            var journalEntries = await dbContext.JournalEntries.ToListAsync();

            var usersCount = users.Count;
            var journalEntriesCount = journalEntries.Count;

            var thoughtUserIndex = 0;
            var thoughtJournalEntryIndex = 0;

            var thoughtFaker = new Faker<ThoughtEntity>()
                .RuleFor(m => m.Details, f => f.Lorem.Sentences(1))
                .RuleFor(m => m.Color, f => f.Internet.Color())
                .RuleFor(m => m.CreationDate, f => f.Date.Between(DateTime.UtcNow.AddMonths(-5), DateTime.UtcNow))
                .RuleFor(m => m.UserId, f => users[DataIndex(ref thoughtUserIndex, usersCount)].Id)
                .RuleFor(m => m.JournalEntryId, f => journalEntries[DataIndex(ref thoughtJournalEntryIndex, journalEntriesCount)].Id);

            var thoughtFakerData = thoughtFaker.GenerateBetween(150, 250);

            dbContext.Thoughts.AddRange(thoughtFakerData);
            await dbContext.SaveChangesAsync();

            Console.WriteLine("ADDED SEED DATA: 'Thoughts'");
        }
    }

    private static async Task<SoarBeyondUserEntity> GetAdminUser(SoarBeyondDbContext dbContext)
        => await dbContext.Users.FirstOrDefaultAsync(u => u.Email.Equals(AdminEmail));

    private static async Task<IdentityRole<int>> GetAdminUserRole(SoarBeyondDbContext dbContext)
        => await dbContext.Roles.FirstOrDefaultAsync(r => r.Name.Equals(AdminRoleName));

    private static int DataIndex(ref int currentIndex, int total)
    {
        return currentIndex++ % total;
    }
}