using JobScheduler.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobScheduler.Infrastructure.Configurations;

public sealed class JobQueueConfiguration : IEntityTypeConfiguration<JobQueue>
{
    public void Configure(EntityTypeBuilder<JobQueue> b)
    {
        b.ToTable("JobQueue");
        b.HasKey(j => j.JobId);


        b.Property(j => j.JobId).ValueGeneratedNever();


        b.Property(j => j.JobType)
        .IsRequired()
        .HasMaxLength(64)
        .HasColumnType("nvarchar(64)");


        b.Property(j => j.Payload)
        .IsRequired()
        .HasColumnType("nvarchar(max)");


        b.Property(j => j.Status)
        .IsRequired()
        .HasConversion<int>()
        .HasColumnType("int");


        b.Property(j => j.Attempts)
        .IsRequired()
        .HasDefaultValue(0);


        b.Property(j => j.AvailableAt).HasColumnType("datetimeoffset");
        b.Property(j => j.LockedAt).HasColumnType("datetimeoffset");


        b.Property(j => j.LockedBy)
        .HasMaxLength(128)
        .HasColumnType("nvarchar(128)");


        // Helpful indexes for queue scanning
        b.HasIndex(j => new { j.Status, j.AvailableAt });
        b.HasIndex(j => j.LockedAt);
    }
}
