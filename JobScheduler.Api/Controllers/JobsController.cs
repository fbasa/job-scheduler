using JobScheduler.Api.Models;
using JobScheduler.Infrastructure;
using JobScheduler.Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobScheduler.Api.Controllers;

[ApiController]
[Route("api/v1/jobs")]
public sealed class JobsController(JobSchedulerDbContext db) : ControllerBase
{
    // GET /api/v1/jobs?status=Pending&take=50
    [HttpGet]
    public async Task<ActionResult<IEnumerable<JobQueue>>> List([FromQuery] JobStatus? status, [FromQuery] int take = 50)
    {
        take = Math.Clamp(take, 1, 500);
        var query = db.JobQueue.AsQueryable();
        if (status is not null)
            query = query.Where(j => j.Status == status);
        var items = await query.OrderBy(j => j.AvailableAt).ThenBy(j => j.JobId).Take(take).ToListAsync();
        return Ok(items);
    }


    // GET /api/v1/jobs/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<JobQueue>> Get(Guid id)
    {
        var job = await db.JobQueue.FindAsync(id);
        return job is null ? NotFound() : Ok(job);
    }


    // POST /api/v1/jobs
    [HttpPost]
    public async Task<ActionResult<JobQueue>> Create([FromBody] CreateJobRequest req)
    {
        var job = new JobQueue
        {
            JobId = Guid.NewGuid(),
            JobType = req.JobType!,
            Payload = req.Payload!,
            Status = JobStatus.Pending,
            Attempts = 0,
            AvailableAt = req.AvailableAt,
            LockedAt = null,
            LockedBy = null
        };
        db.JobQueue.Add(job);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = job.JobId }, job);
    }


    // PATCH /api/v1/jobs/{id}
    [HttpPatch("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateJobRequest req)
    {
        var job = await db.JobQueue.FindAsync(id);
        if (job is null) return NotFound();


        if (req.JobType is not null) job.JobType = req.JobType;
        if (req.Payload is not null) job.Payload = req.Payload;
        if (req.Status is not null) job.Status = req.Status.Value;
        if (req.Attempts is not null) job.Attempts = req.Attempts.Value;
        if (req.AvailableAtChanged) job.AvailableAt = req.AvailableAt;
        if (req.LockedAtChanged) job.LockedAt = req.LockedAt;
        if (req.LockedByChanged) job.LockedBy = req.LockedBy;


        await db.SaveChangesAsync();
        return NoContent();
    }
}
