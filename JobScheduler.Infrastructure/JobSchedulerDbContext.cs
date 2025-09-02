using JobScheduler.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobScheduler.Infrastructure;


public sealed class JobSchedulerDbContext(DbContextOptions<JobSchedulerDbContext> options) : DbContext(options)
{
    public DbSet<JobQueue> JobQueue => Set<JobQueue>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(JobSchedulerDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
