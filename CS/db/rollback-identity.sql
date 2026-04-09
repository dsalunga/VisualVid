-- ============================================================================
-- VisualVid Identity Migration ROLLBACK Script
-- Reverses the migrate-identity.sql changes
-- Run this if the migration needs to be undone.
-- ============================================================================

BEGIN TRANSACTION;

-- Drop new Identity support tables
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'AspNetRoleClaims')
    DROP TABLE [dbo].[AspNetRoleClaims];

IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'AspNetUserTokens')
    DROP TABLE [dbo].[AspNetUserTokens];

IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'AspNetUserLogins')
    DROP TABLE [dbo].[AspNetUserLogins];

IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'AspNetUserClaims')
    DROP TABLE [dbo].[AspNetUserClaims];

-- Remove Identity columns from aspnet_Users
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('aspnet_Users') AND name = 'Email')
    ALTER TABLE [dbo].[aspnet_Users] DROP COLUMN [Email];

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('aspnet_Users') AND name = 'NormalizedEmail')
    ALTER TABLE [dbo].[aspnet_Users] DROP COLUMN [NormalizedEmail];

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('aspnet_Users') AND name = 'EmailConfirmed')
    ALTER TABLE [dbo].[aspnet_Users] DROP COLUMN [EmailConfirmed];

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('aspnet_Users') AND name = 'PasswordHash')
    ALTER TABLE [dbo].[aspnet_Users] DROP COLUMN [PasswordHash];

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('aspnet_Users') AND name = 'SecurityStamp')
    ALTER TABLE [dbo].[aspnet_Users] DROP COLUMN [SecurityStamp];

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('aspnet_Users') AND name = 'ConcurrencyStamp')
    ALTER TABLE [dbo].[aspnet_Users] DROP COLUMN [ConcurrencyStamp];

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('aspnet_Users') AND name = 'PhoneNumber')
    ALTER TABLE [dbo].[aspnet_Users] DROP COLUMN [PhoneNumber];

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('aspnet_Users') AND name = 'PhoneNumberConfirmed')
    ALTER TABLE [dbo].[aspnet_Users] DROP COLUMN [PhoneNumberConfirmed];

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('aspnet_Users') AND name = 'TwoFactorEnabled')
    ALTER TABLE [dbo].[aspnet_Users] DROP COLUMN [TwoFactorEnabled];

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('aspnet_Users') AND name = 'LockoutEnd')
    ALTER TABLE [dbo].[aspnet_Users] DROP COLUMN [LockoutEnd];

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('aspnet_Users') AND name = 'LockoutEnabled')
    ALTER TABLE [dbo].[aspnet_Users] DROP COLUMN [LockoutEnabled];

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('aspnet_Users') AND name = 'AccessFailedCount')
    ALTER TABLE [dbo].[aspnet_Users] DROP COLUMN [AccessFailedCount];

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('aspnet_Users') AND name = 'NormalizedUserName')
    ALTER TABLE [dbo].[aspnet_Users] DROP COLUMN [NormalizedUserName];

-- Remove Identity columns from aspnet_Roles
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('aspnet_Roles') AND name = 'NormalizedName')
    ALTER TABLE [dbo].[aspnet_Roles] DROP COLUMN [NormalizedName];

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('aspnet_Roles') AND name = 'ConcurrencyStamp')
    ALTER TABLE [dbo].[aspnet_Roles] DROP COLUMN [ConcurrencyStamp];

-- Remove new columns from Members
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Members') AND name = 'FirstName')
    ALTER TABLE [dbo].[Members] DROP COLUMN [FirstName];

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Members') AND name = 'LastName')
    ALTER TABLE [dbo].[Members] DROP COLUMN [LastName];

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Members') AND name = 'DateCreated')
    ALTER TABLE [dbo].[Members] DROP COLUMN [DateCreated];

PRINT 'ROLLBACK COMPLETE: All Identity migration changes reversed.';

COMMIT TRANSACTION;
