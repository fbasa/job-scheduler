# EF code first migration

## error when installing dotnet ef
Package Source Mapping is enabled, but no source found under the specified package ID: dotnet-ef. See the documentation for Package Source Mapping at https://aka.ms/nuget-package-source-mapping for more details.

## fixed
In visual studio -> tools -> options -> nuget package manager -> package source mapping -> add -> dotnet*

## check .NET SDK is installed
```dotnet --info```

## install or update the tool
```dotnet tool install --global dotnet-ef```

## or, if already installed:
```dotnet tool update  --global dotnet-ef```

## verify
```dotnet ef --version```


## create migration 
```cd JobScheduler.Infrastructure```  
```dotnet ef migrations add InitialCreate -c JobSchedulerDbContext -o Migrations```  
```dotnet ef migrations add added_LastError -c JobSchedulerDbContext -o Migrations```

## manually update database
```dotnet ef database update -c JobSchedulerDbContext```

## auto update database when API starts DEV only
```using var scope = app.Services.CreateScope();```  
```var db = scope.ServiceProvider.GetRequiredService<JobSchedulerDbContext>();```  
```db.Database.Migrate();```

## open vs code in terminal
```code .```

## open visual studio in terminal
```devenv <solutionname.sln>```

Note:
If not yet added, add this to system environment variables "Path"  
C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE

## apply migration prod
```dotnet ef migrations script -i -o Migrations/Prod/2025-09-03_added_LastError_expand.sql```  
-- -i makes it safe to run even if partially applied  
This will generate script file (for your review) and manually execute in prod.

## prod deployment sequence
In this case (adding LastError as a new column): run the DB script first, then deploy the app.  
Why DB-first here?  
The new app version may start writing LastError. If the column isn't there yet, SaveChanges will fail (Invalid column name 'LastError').  
Old app versions ignore extra columns, so applying the "expand" migration early is harmless.  

Safe sequence (zero-downtime)  
Expand (DB) - run idempotent migration script to add LastError NULL.  

## Squash plan (safe + repeatable)
You can archive/squash your migrations and still be able to spin up brand-new environments from scratch. The pattern is to create a new "baseline" (squashed) migration that represents your current schema, and mark it as applied on existing databases (without running its Up)  
1) Delete all migrations and snapshot.  
2) ```dotnet ef migrations add 0001_Baseline```  
      This produces a migration whose Up() creates the entire schema and a fresh ModelSnapshot. This becomes the only migration the app ships going forward.  
3) Verify it really builds the whole DB (local) then run the API to auto-migrate.  
4) Baseline existing databases (do NOT run Up() there)  
   For prod/staging (already at your current schema), mark the new baseline as applied so EF won't try to run it:  
   ```IF NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory]```  
               ```WHERE [MigrationId] = N'20250903_0001_Baseline')```  
```BEGIN```  
    ```INSERT INTO [__EFMigrationsHistory] ([MigrationId],[ProductVersion])```  
    ```VALUES (N'20250903_0001_Baseline', N'9.0.0');```  
```END```
Note*
When doing baseline, do not include any additional changes to existing state. Do it after steps 1 to 4.  

