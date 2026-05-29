using System.ComponentModel.DataAnnotations;

namespace Audit.Models;

public class Attachment
{
    public int Id { get; set; }

    [Required, MaxLength(255)]
    public string FileName { get; set; } = "";

    [MaxLength(120)]
    public string ContentType { get; set; } = "application/octet-stream";

    public long Size { get; set; }

    public byte[] Data { get; set; } = Array.Empty<byte>();

    public int RequestId { get; set; }
    public Request? Request { get; set; }
}
