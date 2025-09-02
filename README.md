# EF code first migration

## Error when installing dotnet ef
Package Source Mapping is enabled, but no source found under the specified package ID: dotnet-ef. See the documentation for Package Source Mapping at https://aka.ms/nuget-package-source-mapping for more details.

## Fixed
In visual studio -> tools -> options -> nuget package manager -> package source mapping -> add -> dotnet*

## Check .NET SDK is installed
```dotnet --info```

## Install or update the tool
```dotnet tool install --global dotnet-ef```

## or, if already installed:
```dotnet tool update  --global dotnet-ef```

## Verify
```dotnet ef --version```


## create migration 
```cd JobScheduler.Infrastructure```  
```dotnet ef migrations add InitialCreate -c JobSchedulerDbContext -o Migrations```

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

## apply migration PROD 
```dotnet ef migrations script -i -o Migrations/Prod/2025-09-03_added_LastError_expand.sql```  
-- -i makes it safe to run even if partially applied

