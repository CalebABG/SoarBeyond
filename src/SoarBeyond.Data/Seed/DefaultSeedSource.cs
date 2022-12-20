using Bogus;
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

    public async Task Seed(SoarBeyondDbContext context)
    {
        if (!await context.Users.AnyAsync())
        {
            var pwHasher = new BCryptPasswordHasher<UserEntity>();

            var admin = new UserEntity
            {
                Email = AdminEmail,
                CreatedDate = DateTimeOffset.UtcNow,
                NormalizedEmail = AdminEmail.ToUpper(),
                UserName = AdminEmail,
                NormalizedUserName = AdminEmail.ToUpper(),
                EmailConfirmed = true,
                PasswordHash = pwHasher.HashPassword(null, AdminPass),
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = true,
            };

            context.Users.Add(admin);
            await context.SaveChangesAsync();

            Console.WriteLine("ADDED SEED DATA: 'Users'");
        }

        if (!await context.Roles.AnyAsync())
        {
            Console.WriteLine("ADDED SEED DATA: 'Roles'");
            context.Roles.AddRange(new List<IdentityRole<int>>
            {
                new()
                {
                    Name = AdminRoleName,
                    NormalizedName = AdminRoleName.ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }
            });
            await context.SaveChangesAsync();
        }

        if (!await context.UserRoles.AnyAsync())
        {
            var adminUser = await GetAdminUser(context);
            var adminUserRole = await GetAdminUserRole(context);

            context.UserRoles.AddRange(new List<IdentityUserRole<int>>
            {
                new()
                {
                    RoleId = adminUserRole.Id,
                    UserId = adminUser.Id,
                }
            });
            await context.SaveChangesAsync();

            Console.WriteLine("ADDED SEED DATA: 'User Roles'");
        }

        if (!await context.Journals.AnyAsync())
        {
            var adminUser = await GetAdminUser(context);

            var journalsFaker = new Faker<JournalEntity>()
                .RuleFor(m => m.Name, f => f.Lorem.Sentence(4))
                .RuleFor(m => m.Description, f => f.Lorem.Sentences(4))
                .RuleFor(m => m.CreatedDate, f => f.Date.BetweenOffset(DateTimeOffset.UtcNow.AddMonths(-7), DateTimeOffset.UtcNow))
                .RuleFor(m => m.UpdatedDate, f => f.Date.BetweenOffset(DateTimeOffset.UtcNow.AddMonths(-5), DateTimeOffset.UtcNow))
                .RuleFor(m => m.UserId, f => adminUser.Id);

            context.Journals.AddRange(journalsFaker.GenerateBetween(46, 68));
            await context.SaveChangesAsync();

            Console.WriteLine($"ADDED SEED DATA: '{nameof(JournalEntity)}'");
        }

        if (!await context.Moments.AnyAsync())
        {
            var journals = await context.Journals.ToListAsync();
            var journalsCount = journals.Count;
            var journalIndex = 0;

            var momentFaker = new Faker<MomentEntity>()
                .RuleFor(m => m.Title, f => f.Lorem.Sentence(5))
                .RuleFor(m => m.Content, f => f.Lorem.Sentences(3))
                .RuleFor(m => m.Color, f => f.Internet.Color())
                .RuleFor(m => m.CreatedDate, f => f.Date.BetweenOffset(DateTimeOffset.UtcNow.AddMonths(-7), DateTimeOffset.UtcNow))
                .RuleFor(m => m.UpdatedDate, f => f.Date.BetweenOffset(DateTimeOffset.UtcNow.AddMonths(-5), DateTimeOffset.UtcNow))
                .RuleFor(m => m.JournalId, f => journals[DataIndex(ref journalIndex, journalsCount)].Id);

            context.Moments.AddRange(momentFaker.GenerateBetween(95, 145));
            await context.SaveChangesAsync();

            Console.WriteLine($"ADDED SEED DATA: '${nameof(MomentEntity)}'");
        }

        if (!await context.Notes.AnyAsync())
        {
            var moments = await context.Moments.ToListAsync();
            var momentsCount = moments.Count;
            var momentIndex = 0;

            var noteFaker = new Faker<NoteEntity>()
                .RuleFor(m => m.Details, f => f.Lorem.Sentences(2))
                .RuleFor(m => m.CreatedDate, f => f.Date.BetweenOffset(DateTimeOffset.UtcNow.AddMonths(-5), DateTimeOffset.UtcNow))
                .RuleFor(m => m.UpdatedDate, f => f.Date.BetweenOffset(DateTimeOffset.UtcNow.AddMonths(-2), DateTimeOffset.UtcNow))
                .RuleFor(m => m.MomentId, f => moments[DataIndex(ref momentIndex, momentsCount)].Id);

            context.Notes.AddRange(noteFaker.GenerateBetween(150, 250));
            await context.SaveChangesAsync();

            Console.WriteLine($"ADDED SEED DATA: '{nameof(NoteEntity)}'");
        }
    }

    private static async Task<UserEntity> GetAdminUser(SoarBeyondDbContext context)
        => await context.Users.FirstOrDefaultAsync(u => u.Email.Equals(AdminEmail));

    private static async Task<IdentityRole<int>> GetAdminUserRole(SoarBeyondDbContext context)
        => await context.Roles.FirstOrDefaultAsync(r => r.Name.Equals(AdminRoleName));

    private static int DataIndex(ref int currentIndex, int total)
    {
        return currentIndex++ % total;
    }
}