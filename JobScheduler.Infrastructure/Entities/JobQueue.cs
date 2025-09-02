using System.ComponentModel.DataAnnotations;

namespace JobScheduler.Infrastructure.Entities;

public enum JobStatus
{
    Pending = 0,
    InProgress = 1,
    Succeeded = 2,
    Failed = 3
}

public sealed class JobQueue
{
    public Guid JobId { get; set; } // PK


    [Required, StringLength(64)]
    public string JobType { get; set; } = default!;


    [Required]
    public string Payload { get; set; } = default!; // JSON or arbitrary text


    public JobStatus Status { get; set; } = JobStatus.Pending;


    public int Attempts { get; set; } = 0;


    public DateTimeOffset? AvailableAt { get; set; }
    public DateTimeOffset? LockedAt { get; set; }


    [StringLength(128)]
    public string? LockedBy { get; set; }

    [StringLength(500)]
    public string? LastError { get; set; }
}
