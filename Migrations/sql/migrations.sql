IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220120020232_InitialMigration')
BEGIN
    CREATE TABLE [Gateway] (
        [Id] int NOT NULL IDENTITY,
        [SerialNumber] nvarchar(450) NOT NULL,
        [Name] nvarchar(max) NULL,
        [IpAddress] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Gateway] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220120020232_InitialMigration')
BEGIN
    CREATE TABLE [Peripheral] (
        [Id] int NOT NULL IDENTITY,
        [UID] bigint NOT NULL,
        [Vendor] nvarchar(max) NULL,
        [CreationDate] datetime2 NOT NULL,
        [Status] bit NOT NULL,
        [GatewayId] int NOT NULL,
        CONSTRAINT [PK_Peripheral] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Peripheral_Gateway_GatewayId] FOREIGN KEY ([GatewayId]) REFERENCES [Gateway] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220120020232_InitialMigration')
BEGIN
    CREATE UNIQUE INDEX [IX_Gateway_SerialNumber] ON [Gateway] ([SerialNumber]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220120020232_InitialMigration')
BEGIN
    CREATE INDEX [IX_Peripheral_GatewayId] ON [Peripheral] ([GatewayId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220120020232_InitialMigration')
BEGIN
    CREATE UNIQUE INDEX [IX_Peripheral_UID] ON [Peripheral] ([UID]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220120020232_InitialMigration')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220120020232_InitialMigration', N'3.1.22');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220121002938_Detachable_Peripherals')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Peripheral]') AND [c].[name] = N'GatewayId');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Peripheral] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Peripheral] ALTER COLUMN [GatewayId] int NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220121002938_Detachable_Peripherals')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220121002938_Detachable_Peripherals', N'3.1.22');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220121003538_SoftDelete')
BEGIN
    ALTER TABLE [Peripheral] ADD [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220121003538_SoftDelete')
BEGIN
    ALTER TABLE [Gateway] ADD [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220121003538_SoftDelete')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220121003538_SoftDelete', N'3.1.22');
END;

GO

