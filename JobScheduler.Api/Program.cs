using JobScheduler.Infrastructure;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);


// EF Core (SQL Server)
builder.Services.AddDbContext<JobSchedulerDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();


    // Auto-apply EF Core migrations in Development only
    using var scope = app.Services.CreateScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Startup");
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<JobSchedulerDbContext>();
        db.Database.Migrate();
        logger.LogInformation("Applied EF Core migrations in Development.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Failed to apply EF Core migrations");
        throw; // fail fast in dev so issues are visible
    }
}


app.UseHttpsRedirection();
app.MapControllers();
app.Run();