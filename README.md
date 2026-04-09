# VisualVid

VisualVid is a YouTube-like video sharing application modernized to ASP.NET Core on .NET 10.

## Migration Summary

- Web app migrated from ASP.NET Web Forms to ASP.NET Core MVC.
- Authentication migrated to ASP.NET Core Identity (with compatibility for legacy `aspnet_*` tables).
- Video encoding moved to a .NET Worker Service (`BackgroundService`).
- Core helpers/data utilities extracted into a shared SDK-style class library.
- Legacy artifacts archived under `legacy/`.

## Solution Layout

- `CS/VisualVid.sln` - main .NET 10 solution.
- `CS/src/VisualVid.Web` - ASP.NET Core MVC web app (public + member + admin areas).
- `CS/src/VisualVid.Core` - shared domain/helpers/data utilities.
- `CS/src/VisualVid.Encoder` - background video encoder worker.
- `CS/src/VisualVid.Tests` - xUnit test project.
- `CS/db` - database setup and migration scripts.
- `legacy/` - archived Web Forms/VB/deployment assets.

## Tech Stack

- .NET SDK 10
- ASP.NET Core MVC + Areas
- ASP.NET Core Identity
- Entity Framework Core (SQL Server and PostgreSQL support)
- xUnit + Moq
- FFmpeg for background transcoding/thumbnail generation

## Prerequisites

- .NET SDK 10 installed (`dotnet --info`)
- Database engine: PostgreSQL (recommended for local dev in this repo) or SQL Server
- FFmpeg installed and available on `PATH` (or configure explicit path in encoder settings)
- SMTP server (or local relay) for notification emails

## Quick Start (PostgreSQL)

1. Create database/user (example):
```bash
brew install postgresql@17
brew services start postgresql@17
createuser -s visualvid
createdb -O visualvid visualvid
```

2. Initialize schema:
```bash
psql -U visualvid -d visualvid -f CS/db/init-postgres.sql
```

3. Restore/build:
```bash
dotnet restore CS/VisualVid.sln
dotnet build CS/VisualVid.sln
```

4. Run web app:
```bash
dotnet run --project CS/src/VisualVid.Web
```

5. (Optional) Run encoder worker in another terminal:
```bash
dotnet run --project CS/src/VisualVid.Encoder
```

## SQL Server Identity Migration

For SQL Server-backed environments migrating from legacy membership schema:

- Apply: `CS/db/migrate-identity.sql`
- Rollback: `CS/db/rollback-identity.sql`

Run these through your SQL Server tooling (`sqlcmd`, SSMS, Azure Data Studio, etc.) against a backup/staging copy first.

## Configuration

- Web default config (SQL Server): `CS/src/VisualVid.Web/appsettings.json`
- Web development config (PostgreSQL): `CS/src/VisualVid.Web/appsettings.Development.json`
- Encoder default config (SQL Server + file paths): `CS/src/VisualVid.Encoder/appsettings.json`
- Encoder development override (PostgreSQL): `CS/src/VisualVid.Encoder/appsettings.Development.json`

Set the database provider via:

- `DatabaseProvider` for web app
- `Encoder:DatabaseProvider` for encoder worker

Valid values: `SqlServer` or `PostgreSQL`.

## Useful Endpoints

- Health: `/health`
- Metrics (admin role required): `/metrics`

Legacy URL compatibility routes are included for:

- `Watch.aspx`
- `Profile.aspx`
- `Results.aspx`

## Testing

Run all tests:

```bash
dotnet test CS/VisualVid.sln
```

## Notes

- The repository currently references some .NET 10 preview packages.
- `legacy/` is intentionally retained for historical reference and rollback support during transition.
