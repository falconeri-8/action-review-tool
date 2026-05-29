using Audit.Data;
using Audit.Models;
using Microsoft.EntityFrameworkCore;

namespace Audit.Services;

public class RequestService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public RequestService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<List<Request>> ForSubmitterAsync(int submitterId, string? query = null)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var q = db.Requests
            .Include(r => r.Submitter)
            .Include(r => r.Reviewer)
            .Include(r => r.Attachments)
            .Where(r => r.SubmitterId == submitterId);

        q = ApplyQuery(q, query);

        return await q.OrderByDescending(r => r.CreatedAt).ToListAsync();
    }

    public async Task<List<Request>> ForReviewAsync(string? query = null, RequestTag? tag = null)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var q = db.Requests
            .Include(r => r.Submitter)
            .Include(r => r.Reviewer)
            .Include(r => r.Attachments)
            .AsQueryable();

        q = ApplyQuery(q, query);

        var list = await q.OrderBy(r => r.CreatedAt).ToListAsync();

        if (tag is not null)
            list = list.Where(r => r.Tags.Contains(tag.Value)).ToList();

        return list;
    }

    private static IQueryable<Request> ApplyQuery(IQueryable<Request> q, string? query)
    {
        if (string.IsNullOrWhiteSpace(query)) return q;

        var trimmed = query.Trim();
        if (int.TryParse(trimmed, out var id))
            return q.Where(r => r.Id == id || r.Title.Contains(trimmed));

        if (trimmed.StartsWith("REQ-", StringComparison.OrdinalIgnoreCase) &&
            int.TryParse(trimmed.AsSpan(4), out var prefixed))
            return q.Where(r => r.Id == prefixed);

        return q.Where(r => r.Title.Contains(trimmed));
    }

    public async Task<Request?> FindAsync(int id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Requests
            .Include(r => r.Submitter)
            .Include(r => r.Reviewer)
            .Include(r => r.Attachments)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Request> CreateAsync(Request input, IEnumerable<Attachment> attachments)
    {
        await using var db = await _factory.CreateDbContextAsync();
        input.Status = RequestStatus.Pending;
        input.CreatedAt = DateTime.UtcNow;
        input.Attachments = attachments.ToList();
        db.Requests.Add(input);
        await db.SaveChangesAsync();
        return input;
    }

    public async Task DecideAsync(int requestId, int reviewerId, RequestStatus decision, string comment)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var r = await db.Requests.FirstAsync(x => x.Id == requestId);
        r.Status = decision;
        r.ReviewerId = reviewerId;
        r.Comment = comment ?? "";
        r.DecidedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
    }

    public async Task<Attachment?> GetAttachmentAsync(int id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Attachments.FindAsync(id);
    }
}
