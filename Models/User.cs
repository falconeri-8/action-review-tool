using System.ComponentModel.DataAnnotations;

namespace Audit.Models;

public class User
{
    public int Id { get; set; }

    [Required, MaxLength(120)]
    public string FullName { get; set; } = "";

    [MaxLength(160)]
    public string Contact { get; set; } = "";

    public Play Play { get; set; } = Play.Contributor;

    [MaxLength(80)]
    public string Role { get; set; } = "";

    [Required, MaxLength(64)]
    public string Password { get; set; } = "";

    public byte[]? Photo { get; set; }

    public string? PhotoContentType { get; set; }
}
