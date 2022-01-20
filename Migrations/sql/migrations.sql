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

