-- VisualVid PostgreSQL Schema
-- Run this to initialize a fresh PostgreSQL database for VisualVid on macOS/Linux.
-- Usage:
--   brew install postgresql@17 && brew services start postgresql@17
--   createuser -s visualvid
--   createdb -O visualvid visualvid
--   psql -U visualvid -d visualvid -f CS/db/init-postgres.sql

BEGIN;

-- Identity: Users
CREATE TABLE IF NOT EXISTS aspnet_users (
    user_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_name VARCHAR(256),
    lowered_user_name VARCHAR(256),
    "Email" VARCHAR(256),
    "NormalizedEmail" VARCHAR(256),
    "EmailConfirmed" BOOLEAN NOT NULL DEFAULT FALSE,
    "PasswordHash" TEXT,
    "SecurityStamp" TEXT,
    "ConcurrencyStamp" TEXT,
    "PhoneNumber" TEXT,
    "PhoneNumberConfirmed" BOOLEAN NOT NULL DEFAULT FALSE,
    "TwoFactorEnabled" BOOLEAN NOT NULL DEFAULT FALSE,
    "LockoutEnd" TIMESTAMPTZ,
    "LockoutEnabled" BOOLEAN NOT NULL DEFAULT FALSE,
    "AccessFailedCount" INTEGER NOT NULL DEFAULT 0
);

CREATE UNIQUE INDEX IF NOT EXISTS ix_aspnet_users_lowered ON aspnet_users (lowered_user_name);

-- Identity: Roles
CREATE TABLE IF NOT EXISTS aspnet_roles (
    role_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    role_name VARCHAR(256),
    lowered_role_name VARCHAR(256),
    "ConcurrencyStamp" TEXT
);

-- Identity: UserRoles
CREATE TABLE IF NOT EXISTS aspnet_users_in_roles (
    "UserId" UUID NOT NULL REFERENCES aspnet_users(user_id) ON DELETE CASCADE,
    "RoleId" UUID NOT NULL REFERENCES aspnet_roles(role_id) ON DELETE CASCADE,
    PRIMARY KEY ("UserId", "RoleId")
);

-- Identity: UserClaims
CREATE TABLE IF NOT EXISTS aspnet_user_claims (
    "Id" SERIAL PRIMARY KEY,
    "UserId" UUID NOT NULL REFERENCES aspnet_users(user_id) ON DELETE CASCADE,
    "ClaimType" TEXT,
    "ClaimValue" TEXT
);

-- Identity: UserLogins
CREATE TABLE IF NOT EXISTS aspnet_user_logins (
    "LoginProvider" VARCHAR(128) NOT NULL,
    "ProviderKey" VARCHAR(128) NOT NULL,
    "ProviderDisplayName" TEXT,
    "UserId" UUID NOT NULL REFERENCES aspnet_users(user_id) ON DELETE CASCADE,
    PRIMARY KEY ("LoginProvider", "ProviderKey")
);

-- Identity: RoleClaims
CREATE TABLE IF NOT EXISTS aspnet_role_claims (
    "Id" SERIAL PRIMARY KEY,
    "RoleId" UUID NOT NULL REFERENCES aspnet_roles(role_id) ON DELETE CASCADE,
    "ClaimType" TEXT,
    "ClaimValue" TEXT
);

-- Identity: UserTokens
CREATE TABLE IF NOT EXISTS aspnet_user_tokens (
    "UserId" UUID NOT NULL REFERENCES aspnet_users(user_id) ON DELETE CASCADE,
    "LoginProvider" VARCHAR(128) NOT NULL,
    "Name" VARCHAR(128) NOT NULL,
    "Value" TEXT,
    PRIMARY KEY ("UserId", "LoginProvider", "Name")
);

-- Application: Categories
CREATE TABLE IF NOT EXISTS "Categories" (
    "CategoryID" SERIAL PRIMARY KEY,
    "Name" VARCHAR(255) NOT NULL
);

-- Application: Countries
CREATE TABLE IF NOT EXISTS "Countries" (
    "CountryCode" INTEGER PRIMARY KEY,
    "Name" VARCHAR(255)
);

INSERT INTO "Countries" ("CountryCode", "Name") VALUES
(1, 'United States'), (7, 'Russia'), (20, 'Egypt'), (27, 'South Africa'),
(30, 'Greece'), (31, 'Netherlands'), (32, 'Belgium'), (33, 'France'),
(34, 'Spain'), (36, 'Hungary'), (39, 'Italy'), (40, 'Romania'),
(41, 'Switzerland'), (43, 'Austria'), (44, 'United Kingdom'), (45, 'Denmark'),
(46, 'Sweden'), (47, 'Norway'), (48, 'Poland'), (49, 'Germany'),
(51, 'Peru'), (52, 'Mexico'), (53, 'Cuba'), (54, 'Argentina'),
(55, 'Brazil'), (56, 'Chile'), (57, 'Colombia'), (58, 'Venezuela'),
(60, 'Malaysia'), (61, 'Australia'), (62, 'Indonesia'), (63, 'Philippines'),
(64, 'New Zealand'), (65, 'Singapore'), (66, 'Thailand'), (81, 'Japan'),
(82, 'South Korea'), (84, 'Vietnam'), (86, 'China'), (90, 'Turkey'),
(91, 'India'), (92, 'Pakistan'), (93, 'Afghanistan'), (94, 'Sri Lanka'),
(95, 'Myanmar'), (98, 'Iran'), (212, 'Morocco'), (213, 'Algeria'),
(216, 'Tunisia'), (218, 'Libya'), (234, 'Nigeria'), (254, 'Kenya'),
(255, 'Tanzania'), (256, 'Uganda'), (260, 'Zambia'), (263, 'Zimbabwe'),
(353, 'Ireland'), (354, 'Iceland'), (358, 'Finland'), (370, 'Lithuania'),
(371, 'Latvia'), (372, 'Estonia'), (380, 'Ukraine'), (420, 'Czech Republic'),
(421, 'Slovakia'), (852, 'Hong Kong'), (853, 'Macau'), (855, 'Cambodia'),
(880, 'Bangladesh'), (886, 'Taiwan'), (960, 'Maldives'), (961, 'Lebanon'),
(962, 'Jordan'), (963, 'Syria'), (964, 'Iraq'), (965, 'Kuwait'),
(966, 'Saudi Arabia'), (967, 'Yemen'), (968, 'Oman'),
(971, 'United Arab Emirates'), (972, 'Israel'), (974, 'Qatar'),
(977, 'Nepal'), (992, 'Tajikistan'), (993, 'Turkmenistan'),
(994, 'Azerbaijan'), (995, 'Georgia'), (996, 'Kyrgyzstan'), (998, 'Uzbekistan')
ON CONFLICT DO NOTHING;

-- Application: Videos
CREATE TABLE IF NOT EXISTS "Videos" (
    "VideoId" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "Title" VARCHAR(255),
    "Description" VARCHAR(4000),
    "Tags" VARCHAR(4000),
    "CategoryID" INTEGER REFERENCES "Categories"("CategoryID"),
    "UserId" UUID REFERENCES aspnet_users(user_id),
    "DateAdded" TIMESTAMPTZ DEFAULT NOW(),
    "Views" INTEGER NOT NULL DEFAULT 0,
    "Ratings" INTEGER NOT NULL DEFAULT 0,
    "RatingTicks" INTEGER NOT NULL DEFAULT 0,
    "Length" INTEGER NOT NULL DEFAULT 0,
    "IsActive" BOOLEAN NOT NULL DEFAULT FALSE,
    "Pending" BOOLEAN NOT NULL DEFAULT TRUE,
    "OriginalExtension" VARCHAR(64)
);

-- Application: Members
CREATE TABLE IF NOT EXISTS "Members" (
    "UserId" UUID PRIMARY KEY REFERENCES aspnet_users(user_id),
    "CountryCode" INTEGER REFERENCES "Countries"("CountryCode"),
    "Gender" BOOLEAN NOT NULL DEFAULT FALSE,
    "BirthDate" TIMESTAMPTZ,
    "Watched" INTEGER NOT NULL DEFAULT 0,
    "FirstName" VARCHAR(100),
    "LastName" VARCHAR(100),
    "DateCreated" TIMESTAMPTZ DEFAULT NOW()
);

-- Application: Comments
CREATE TABLE IF NOT EXISTS "Comments" (
    "CommentID" SERIAL PRIMARY KEY,
    "VideoId" UUID REFERENCES "Videos"("VideoId") ON DELETE CASCADE,
    "UserId" UUID REFERENCES aspnet_users(user_id),
    "Content" TEXT,
    "DatePosted" TIMESTAMPTZ DEFAULT NOW(),
    "ReplyFromID" INTEGER
);

-- Seed: Default categories
INSERT INTO "Categories" ("Name") VALUES
    ('Entertainment'), ('Education'), ('Music'), ('Sports'), ('Technology'),
    ('News'), ('Comedy'), ('Gaming'), ('Travel'), ('Other')
ON CONFLICT DO NOTHING;

-- Seed: Default admin role
INSERT INTO aspnet_roles (role_id, role_name, lowered_role_name, "ConcurrencyStamp")
VALUES (gen_random_uuid(), 'Administrators', 'administrators', gen_random_uuid()::text)
ON CONFLICT DO NOTHING;

COMMIT;
