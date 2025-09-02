using System.ComponentModel.DataAnnotations;

namespace JobScheduler.Api.Models;

public sealed class CreateJobRequest
{
    [Required, StringLength(64)] public string? JobType { get; init; }
    [Required] public string? Payload { get; init; }
    public DateTimeOffset? AvailableAt { get; init; }
}