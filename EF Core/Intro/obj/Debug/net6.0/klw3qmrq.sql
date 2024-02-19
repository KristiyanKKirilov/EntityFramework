BEGIN TRANSACTION;
GO

ALTER TABLE [Addresses] ADD [AppartmentNumber] nvarchar(10) NOT NULL DEFAULT N'';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231111142556_AppartmentNumberAdded', N'7.0.13');
GO

COMMIT;
GO

