-- ============================================================================
-- VisualVid Identity Migration Script
-- Migrates from ASP.NET Membership (aspnet_*) to ASP.NET Core Identity schema
-- 
-- IMPORTANT: Run this in a transaction on a BACKUP of the database first.
-- Verify results before applying to production.
-- ============================================================================

-- ============================================================================
-- STEP 1: Add ASP.NET Core Identity columns to aspnet_Users
-- These columns are required by ASP.NET Core Identity but don't exist in legacy
-- ============================================================================
BEGIN TRANSACTION;

-- Add Identity-required columns to aspnet_Users
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('aspnet_Users') AND name = 'Email')
    ALTER TABLE [dbo].[aspnet_Users] ADD [Email] NVARCHAR(256) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('aspnet_Users') AND name = 'NormalizedEmail')
    ALTER TABLE [dbo].[aspnet_Users] ADD [NormalizedEmail] NVARCHAR(256) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('aspnet_Users') AND name = 'EmailConfirmed')
    ALTER TABLE [dbo].[aspnet_Users] ADD [EmailConfirmed] BIT NOT NULL DEFAULT(0);

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('aspnet_Users') AND name = 'PasswordHash')
    ALTER TABLE [dbo].[aspnet_Users] ADD [PasswordHash] NVARCHAR(MAX) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('aspnet_Users') AND name = 'SecurityStamp')
    ALTER TABLE [dbo].[aspnet_Users] ADD [SecurityStamp] NVARCHAR(MAX) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('aspnet_Users') AND name = 'ConcurrencyStamp')
    ALTER TABLE [dbo].[aspnet_Users] ADD [ConcurrencyStamp] NVARCHAR(MAX) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('aspnet_Users') AND name = 'PhoneNumber')
    ALTER TABLE [dbo].[aspnet_Users] ADD [PhoneNumber] NVARCHAR(MAX) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('aspnet_Users') AND name = 'PhoneNumberConfirmed')
    ALTER TABLE [dbo].[aspnet_Users] ADD [PhoneNumberConfirmed] BIT NOT NULL DEFAULT(0);

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('aspnet_Users') AND name = 'TwoFactorEnabled')
    ALTER TABLE [dbo].[aspnet_Users] ADD [TwoFactorEnabled] BIT NOT NULL DEFAULT(0);

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('aspnet_Users') AND name = 'LockoutEnd')
    ALTER TABLE [dbo].[aspnet_Users] ADD [LockoutEnd] DATETIMEOFFSET NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('aspnet_Users') AND name = 'LockoutEnabled')
    ALTER TABLE [dbo].[aspnet_Users] ADD [LockoutEnabled] BIT NOT NULL DEFAULT(1);

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('aspnet_Users') AND name = 'AccessFailedCount')
    ALTER TABLE [dbo].[aspnet_Users] ADD [AccessFailedCount] INT NOT NULL DEFAULT(0);

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('aspnet_Users') AND name = 'NormalizedUserName')
    ALTER TABLE [dbo].[aspnet_Users] ADD [NormalizedUserName] NVARCHAR(256) NULL;

PRINT 'STEP 1 COMPLETE: Identity columns added to aspnet_Users';

-- ============================================================================
-- STEP 2: Populate Identity columns from aspnet_Membership data
-- ============================================================================

-- Copy Email from aspnet_Membership to aspnet_Users
UPDATE u SET
    u.Email = m.Email,
    u.NormalizedEmail = UPPER(m.Email),
    u.EmailConfirmed = m.IsApproved,
    u.SecurityStamp = NEWID(),
    u.ConcurrencyStamp = NEWID(),
    u.LockoutEnabled = 1,
    u.AccessFailedCount = m.FailedPasswordAttemptCount,
    u.LockoutEnd = CASE WHEN m.IsLockedOut = 1 THEN CAST(m.LastLockoutDate AS DATETIMEOFFSET) ELSE NULL END,
    u.NormalizedUserName = u.LoweredUserName
FROM [dbo].[aspnet_Users] u
INNER JOIN [dbo].[aspnet_Membership] m ON u.UserId = m.UserId;

-- Convert legacy password hashes to ASP.NET Core Identity format
-- ASP.NET Membership uses: PasswordFormat 0=Clear, 1=Hashed (SHA1), 2=Encrypted
-- ASP.NET Core Identity v3 uses PBKDF2-HMAC-SHA256
-- Since legacy hashes are SHA1-based, users MUST reset passwords after migration.
-- We set a marker hash that forces password reset on first login.
UPDATE u SET
    u.PasswordHash = 'LEGACY_MIGRATION_REQUIRES_RESET'
FROM [dbo].[aspnet_Users] u
INNER JOIN [dbo].[aspnet_Membership] m ON u.UserId = m.UserId
WHERE m.PasswordFormat = 1; -- Hashed passwords cannot be converted

-- For clear-text passwords (PasswordFormat=0), hash them with Identity's hasher
-- This would need to be done in application code. Mark them similarly.
UPDATE u SET
    u.PasswordHash = 'LEGACY_MIGRATION_REQUIRES_RESET'
FROM [dbo].[aspnet_Users] u
INNER JOIN [dbo].[aspnet_Membership] m ON u.UserId = m.UserId
WHERE m.PasswordFormat = 0;

PRINT 'STEP 2 COMPLETE: Identity columns populated from aspnet_Membership';

-- ============================================================================
-- STEP 3: Add Identity-required columns to aspnet_Roles
-- ============================================================================

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('aspnet_Roles') AND name = 'NormalizedName')
    ALTER TABLE [dbo].[aspnet_Roles] ADD [NormalizedName] NVARCHAR(256) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('aspnet_Roles') AND name = 'ConcurrencyStamp')
    ALTER TABLE [dbo].[aspnet_Roles] ADD [ConcurrencyStamp] NVARCHAR(MAX) NULL;

-- Populate NormalizedName from LoweredRoleName
UPDATE [dbo].[aspnet_Roles] SET
    NormalizedName = LoweredRoleName,
    ConcurrencyStamp = NEWID();

PRINT 'STEP 3 COMPLETE: Identity columns added and populated on aspnet_Roles';

-- ============================================================================
-- STEP 4: Create Identity support tables that don't exist in legacy
-- ============================================================================

-- AspNetUserClaims
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'AspNetUserClaims')
BEGIN
    CREATE TABLE [dbo].[AspNetUserClaims] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [UserId] UNIQUEIDENTIFIER NOT NULL,
        [ClaimType] NVARCHAR(MAX) NULL,
        [ClaimValue] NVARCHAR(MAX) NULL,
        CONSTRAINT [FK_AspNetUserClaims_aspnet_Users_UserId]
            FOREIGN KEY ([UserId]) REFERENCES [dbo].[aspnet_Users]([UserId]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]([UserId]);
    PRINT 'Created AspNetUserClaims table';
END

-- AspNetUserLogins
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'AspNetUserLogins')
BEGIN
    CREATE TABLE [dbo].[AspNetUserLogins] (
        [LoginProvider] NVARCHAR(128) NOT NULL,
        [ProviderKey] NVARCHAR(128) NOT NULL,
        [ProviderDisplayName] NVARCHAR(MAX) NULL,
        [UserId] UNIQUEIDENTIFIER NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_aspnet_Users_UserId]
            FOREIGN KEY ([UserId]) REFERENCES [dbo].[aspnet_Users]([UserId]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]([UserId]);
    PRINT 'Created AspNetUserLogins table';
END

-- AspNetUserTokens
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'AspNetUserTokens')
BEGIN
    CREATE TABLE [dbo].[AspNetUserTokens] (
        [UserId] UNIQUEIDENTIFIER NOT NULL,
        [LoginProvider] NVARCHAR(128) NOT NULL,
        [Name] NVARCHAR(128) NOT NULL,
        [Value] NVARCHAR(MAX) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_aspnet_Users_UserId]
            FOREIGN KEY ([UserId]) REFERENCES [dbo].[aspnet_Users]([UserId]) ON DELETE CASCADE
    );
    PRINT 'Created AspNetUserTokens table';
END

-- AspNetRoleClaims
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'AspNetRoleClaims')
BEGIN
    CREATE TABLE [dbo].[AspNetRoleClaims] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [RoleId] UNIQUEIDENTIFIER NOT NULL,
        [ClaimType] NVARCHAR(MAX) NULL,
        [ClaimValue] NVARCHAR(MAX) NULL,
        CONSTRAINT [FK_AspNetRoleClaims_aspnet_Roles_RoleId]
            FOREIGN KEY ([RoleId]) REFERENCES [dbo].[aspnet_Roles]([RoleId]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]([RoleId]);
    PRINT 'Created AspNetRoleClaims table';
END

PRINT 'STEP 4 COMPLETE: Identity support tables created';

-- ============================================================================
-- STEP 5: Add Members table columns needed by the new app
-- ============================================================================

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Members') AND name = 'FirstName')
    ALTER TABLE [dbo].[Members] ADD [FirstName] NVARCHAR(256) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Members') AND name = 'LastName')
    ALTER TABLE [dbo].[Members] ADD [LastName] NVARCHAR(256) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Members') AND name = 'DateCreated')
    ALTER TABLE [dbo].[Members] ADD [DateCreated] DATETIME NULL;

-- Backfill DateCreated from aspnet_Membership.CreateDate
UPDATE mem SET
    mem.DateCreated = m.CreateDate
FROM [dbo].[Members] mem
INNER JOIN [dbo].[aspnet_Membership] m ON mem.UserId = m.UserId
WHERE mem.DateCreated IS NULL;

PRINT 'STEP 5 COMPLETE: Members table extended with new columns';

-- ============================================================================
-- STEP 6: Verification queries
-- ============================================================================

SELECT 'Users migrated' AS [Check], COUNT(*) AS [Count]
FROM [dbo].[aspnet_Users] WHERE Email IS NOT NULL;

SELECT 'Users needing password reset' AS [Check], COUNT(*) AS [Count]
FROM [dbo].[aspnet_Users] WHERE PasswordHash = 'LEGACY_MIGRATION_REQUIRES_RESET';

SELECT 'Roles migrated' AS [Check], COUNT(*) AS [Count]
FROM [dbo].[aspnet_Roles] WHERE NormalizedName IS NOT NULL;

SELECT 'User-Role mappings' AS [Check], COUNT(*) AS [Count]
FROM [dbo].[aspnet_UsersInRoles];

PRINT 'STEP 6 COMPLETE: Verification queries executed';
PRINT '';
PRINT '=== MIGRATION COMPLETE ===';
PRINT 'IMPORTANT: All users with hashed passwords will need to use "Forgot Password"';
PRINT 'to set a new password on first login after the migration.';
PRINT '';

COMMIT TRANSACTION;
