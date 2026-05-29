using Audit.Components;
using Audit.Data;
using Audit.Services;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var connection = builder.Configuration.GetConnectionString("Default")
    ?? "Data Source=audit.db";

builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseSqlite(connection));

builder.Services.AddScoped<SessionState>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RequestService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
    await using var db = await factory.CreateDbContextAsync();
    await DbSeeder.RunAsync(db);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapGet("/files/{id:int}", async (int id, RequestService svc) =>
{
    var a = await svc.GetAttachmentAsync(id);
    return a is null
        ? Results.NotFound()
        : Results.File(a.Data, a.ContentType, a.FileName);
});

app.Run();
