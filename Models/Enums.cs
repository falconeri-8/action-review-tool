namespace Audit.Models;

public enum Play
{
    Contributor,
    Reviewer,
    Admin
}

public enum RequestStatus
{
    Pending,
    Approved,
    Rejected
}

public enum RequestTag
{
    Code,
    Design,
    Marketing,
    Report,
    Critical
}
