# job-scheduler
EF code first migration poc



# Check .NET SDK is installed
dotnet --info

# Install or update the tool
dotnet tool install --global dotnet-ef

# or, if already installed:
dotnet tool update  --global dotnet-ef

# Verify
dotnet ef --version


dotnet ef migrations add InitialCreate \
  -p ./JobScheduler.Infrastructure \
  -s ../JobScheduler.Api \
  -c JobSchedulerDbContext \
  -o Migrations

dotnet ef database update \
  -p ./JobScheduler.Infrastructure \
  -s ../JobScheduler.Api \
  -c JobSchedulerDbContext



cd JobScheduler.Infrastructure
# create the initial migration (scaffolded locally for your exact environment)



# apply to database
dotnet ef database update -p ./JobScheduler.Infrastructure.csproj -s ../JobScheduler.Api/JobScheduler.Api.csproj -c JobSchedulerDbContext
dotnet ef database update -p .JobScheduler.Infrastructure		  -s src/JobScheduler.Api						-c JobSchedulerDbContext


