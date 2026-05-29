using Audit.Data;
using Audit.Models;
using Microsoft.EntityFrameworkCore;

namespace Audit.Services;

public class AuthService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public AuthService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<User?> AuthenticateAsync(int id, string password)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
        return user is not null && user.Password == password ? user : null;
    }
}
