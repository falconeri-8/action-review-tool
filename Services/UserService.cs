using Audit.Data;
using Audit.Models;
using Microsoft.EntityFrameworkCore;

namespace Audit.Services;

public class UserService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public UserService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<List<User>> ListAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Users.OrderBy(u => u.Id).ToListAsync();
    }

    public async Task<User?> FindAsync(int id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Users.FindAsync(id);
    }

    public async Task<User> CreateAsync(User input)
    {
        await using var db = await _factory.CreateDbContextAsync();
        if (string.IsNullOrWhiteSpace(input.Password)) input.Password = "pass";
        db.Users.Add(input);
        await db.SaveChangesAsync();
        return input;
    }
}
