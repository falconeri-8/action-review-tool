using Audit.Models;
using Microsoft.EntityFrameworkCore;

namespace Audit.Data;

public static class DbSeeder
{
    public static async Task RunAsync(AppDbContext db)
    {
        await db.Database.MigrateAsync();

        if (!await db.Users.AnyAsync())
        {
            db.Users.AddRange(
                new User { FullName = "ADMIN", Contact = "admin@audit.local", Play = Play.Admin, Role = "Operations", Password = "admin" },
                new User { FullName = "REEVES, ELEANOR", Contact = "eleanor@audit.local", Play = Play.Contributor, Role = "Engineering", Password = "pass" },
                new User { FullName = "WALSH, DOMINIC", Contact = "dominic@audit.local", Play = Play.Contributor, Role = "Design", Password = "pass" },
                new User { FullName = "PARK, SEONA", Contact = "seona@audit.local", Play = Play.Reviewer, Role = "Engineering Lead", Password = "pass" },
                new User { FullName = "OKAFOR, JAMAL", Contact = "jamal@audit.local", Play = Play.Reviewer, Role = "Quality Assurance", Password = "pass" }
            );
            await db.SaveChangesAsync();
        }

        if (!await db.Requests.AnyAsync())
        {
            var eleanor = await db.Users.FirstAsync(u => u.FullName == "REEVES, ELEANOR");
            var dominic = await db.Users.FirstAsync(u => u.FullName == "WALSH, DOMINIC");
            var seona = await db.Users.FirstAsync(u => u.FullName == "PARK, SEONA");

            db.Requests.AddRange(
                new Request
                {
                    Title = "Refactor authentication module",
                    Description = "Reduce coupling between auth and session layers; introduce strategy pattern.",
                    Tags = new() { RequestTag.Code, RequestTag.Critical },
                    SubmitterId = eleanor.Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    Status = RequestStatus.Pending
                },
                new Request
                {
                    Title = "Brand palette revision Q3",
                    Description = "Adjust secondary palette to align with new positioning. Includes mockups.",
                    Tags = new() { RequestTag.Design, RequestTag.Marketing },
                    SubmitterId = dominic.Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    Status = RequestStatus.Pending
                },
                new Request
                {
                    Title = "Quarterly incident report",
                    Description = "Compiled report covering downtime causes, MTTR, and remediations.",
                    Tags = new() { RequestTag.Report },
                    SubmitterId = eleanor.Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-9),
                    Status = RequestStatus.Approved,
                    DecidedAt = DateTime.UtcNow.AddDays(-7),
                    ReviewerId = seona.Id,
                    Comment = "Approved. Strong analysis."
                }
            );
            await db.SaveChangesAsync();
        }
    }
}
