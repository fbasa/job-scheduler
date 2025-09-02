using JobScheduler.Infrastructure.Entities;
using System.ComponentModel.DataAnnotations;

namespace JobScheduler.Api.Models;

public sealed class UpdateJobRequest
{
    [StringLength(64)] public string? JobType { get; init; }
    public string? Payload { get; init; }
    public JobStatus? Status { get; init; }
    public int? Attempts { get; init; }
    public DateTimeOffset? AvailableAt { get; init; }
    public bool AvailableAtChanged => AvailableAt.HasValue || AvailableAt is null;
    public DateTimeOffset? LockedAt { get; init; }
    public bool LockedAtChanged => LockedAt.HasValue || LockedAt is null;
    [StringLength(128)] public string? LockedBy { get; init; }
    public bool LockedByChanged => LockedBy is not null;
}

