IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250902215255_InitialCreate'
)
BEGIN
    CREATE TABLE [JobQueue] (
        [JobId] uniqueidentifier NOT NULL,
        [JobType] nvarchar(64) NOT NULL,
        [Payload] nvarchar(max) NOT NULL,
        [Status] int NOT NULL,
        [Attempts] int NOT NULL DEFAULT 0,
        [AvailableAt] datetimeoffset NULL,
        [LockedAt] datetimeoffset NULL,
        [LockedBy] nvarchar(128) NULL,
        CONSTRAINT [PK_JobQueue] PRIMARY KEY ([JobId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250902215255_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_JobQueue_LockedAt] ON [JobQueue] ([LockedAt]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250902215255_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_JobQueue_Status_AvailableAt] ON [JobQueue] ([Status], [AvailableAt]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250902215255_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250902215255_InitialCreate', N'9.0.8');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250902223857_added_LastError'
)
BEGIN
    ALTER TABLE [JobQueue] ADD [LastError] nvarchar(500) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250902223857_added_LastError'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250902223857_added_LastError', N'9.0.8');
END;

COMMIT;
GO

