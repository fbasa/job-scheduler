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
This will generate script file that (review if needed) and manually execute in prod.

## prod deployment sequence
In this case (adding LastError as a new column): run the DB script first, then deploy the app.  
Why DB-first here?  
The new app version may start writing LastError. If the column isn't there yet, SaveChanges will fail (Invalid column name 'LastError').  
Old app versions ignore extra columns, so applying the "expand" migration early is harmless.  

Safe sequence (zero-downtime)  
Expand (DB) - run idempotent migration script to add LastError NULL.  

