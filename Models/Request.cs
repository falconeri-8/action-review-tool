using System.ComponentModel.DataAnnotations;

namespace Audit.Models;

public class Request
{
    public int Id { get; set; }

    [Required, MaxLength(160)]
    public string Title { get; set; } = "";

    public RequestStatus Status { get; set; } = RequestStatus.Pending;

    [MaxLength(4000)]
    public string Description { get; set; } = "";

    public List<RequestTag> Tags { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? DecidedAt { get; set; }

    public int SubmitterId { get; set; }
    public User? Submitter { get; set; }

    public int? ReviewerId { get; set; }
    public User? Reviewer { get; set; }

    [MaxLength(2000)]
    public string Comment { get; set; } = "";

    public List<Attachment> Attachments { get; set; } = new();

    public string Code => $"REQ-{Id:D4}";
}
