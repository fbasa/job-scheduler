using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace JobScheduler.Infrastructure.DesignTime;

public sealed class JobSchedulerDbContextFactory : IDesignTimeDbContextFactory<JobSchedulerDbContext>
{
    public JobSchedulerDbContext CreateDbContext(string[] args)
    {
        var apiDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../JobScheduler.Api"));

        // Try to read from appsettings if available; otherwise fall back to localdb
        var config = new ConfigurationBuilder()
                 .SetBasePath(apiDir)
                 .AddJsonFile("appsettings.json", optional: true)
                 .AddJsonFile("appsettings.Development.json", optional: true)
                 // Use the UserSecretsId from JobScheduler.Api.csproj (after 'dotnet user-secrets init')
                 .AddUserSecrets("4f7174ff-c40b-4d4c-8f1c-11eeba2d600c")
                 //.AddEnvironmentVariables()
                 .Build();

        var cs = config.GetConnectionString("DefaultConnection");

        var options = new DbContextOptionsBuilder<JobSchedulerDbContext>()
        .UseSqlServer(cs)
        .Options;


        return new JobSchedulerDbContext(options);
    }
}
