using Audit.Models;
using Microsoft.EntityFrameworkCore;

namespace Audit.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Request> Requests => Set<Request>();
    public DbSet<Attachment> Attachments => Set<Attachment>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Request>()
            .Property(r => r.Tags)
            .HasConversion(
                v => string.Join(',', v.Select(t => t.ToString())),
                v => string.IsNullOrWhiteSpace(v)
                    ? new List<RequestTag>()
                    : v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => Enum.Parse<RequestTag>(s))
                        .ToList(),
                new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<List<RequestTag>>(
                    (a, c) => (a ?? new()).SequenceEqual(c ?? new()),
                    v => v.Aggregate(0, (h, t) => HashCode.Combine(h, t.GetHashCode())),
                    v => v.ToList()));

        b.Entity<Request>()
            .HasOne(r => r.Submitter)
            .WithMany()
            .HasForeignKey(r => r.SubmitterId)
            .OnDelete(DeleteBehavior.Restrict);

        b.Entity<Request>()
            .HasOne(r => r.Reviewer)
            .WithMany()
            .HasForeignKey(r => r.ReviewerId)
            .OnDelete(DeleteBehavior.SetNull);

        b.Entity<Attachment>()
            .HasOne(a => a.Request)
            .WithMany(r => r.Attachments)
            .HasForeignKey(a => a.RequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
