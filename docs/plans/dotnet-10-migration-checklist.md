# VisualVid .NET 10 Migration Checklist

## Goal
- [x] Migrate VisualVid from ASP.NET Web Forms on .NET Framework 4.5 to ASP.NET Core on .NET 10 with controlled risk and minimal downtime.

## Ground Rules
- [x] Keep production behavior stable while migrating.
- [x] Use phased delivery: Release A (stabilize), Release B (platform migration).
- [x] Preserve existing database behavior first; defer deep schema changes until later phases.
- [x] Maintain rollback path for every release.

## Workstream Snapshot

| Workstream | Target | Effort | Risk |
|---|---|---:|---:|
| Web app (`CS/www`) | ASP.NET Core 10 (Razor Pages or MVC) | 8-12 weeks | High |
| Shared libs (`CS/DES`, `CS/DES.VisualVid`) | SDK-style libraries (`net10.0`) | 1-2 weeks | Medium |
| Editor integration (`CS/FCKeditor.Net_2.2`) | Replace with modern editor | 1-2 weeks | High |
| Encoder (`CS/DES.VisualVid.Process`) | .NET 10 Worker Service | 1-2 weeks | Medium |
| Database project (`CS/VisualVid.Database`) | Keep schema stable, then auth/schema transition | 2-4 weeks | High |
| VB duplicate tree (`VB/*`) | Freeze/retire | 1-2 days | Low |

## Phase 0: Baseline and Discovery
- [x] Capture current environment matrix (IIS version, SQL version, deployment scripts, secrets flow).
- [x] Inventory all user-facing pages, admin pages, handlers, and jobs.
- [x] Build feature regression checklist:
- [x] Authentication/login/logout/register/confirm/password reset/change.
- [x] Member upload/edit/delete/video management.
- [x] Browse/search/watch/comments/profile.
- [x] CMS/admin pages and query tools.
- [x] Capture DB baseline snapshot and backup/restore rehearsal.
- [x] Define SLIs/SLOs and migration success criteria.

## Phase 1: Stabilize on .NET Framework 4.8 (Release A)
- [ ] Create migration branch and CI build for current app.
- [ ] Retarget .NET Framework projects from v4.5 to v4.8.
- [ ] Validate local build on Windows with required targeting packs.
- [ ] Smoke-test all features from Phase 0 checklist.
- [ ] Deploy to staging IIS and run full regression.
- [ ] Release A to production with rollback script validated.

## Phase 2: Extract Portable Core Logic
- [x] Create new SDK-style solution folder for migration artifacts.
- [x] Move portable logic from `DES` and `DES.VisualVid` into new class libraries.
- [x] Replace `System.Web` couplings with ASP.NET Core-friendly abstractions/interfaces.
- [x] Add automated tests for extracted domain/data helpers (66 xUnit tests: Validator, HashHelper, FileHelper, VideoHelper, VideoService, MemberService).
- [x] Keep old app consuming same logic where feasible to reduce drift.

## Phase 3: Stand Up ASP.NET Core 10 Host
- [x] Create ASP.NET Core 10 web project.
- [x] Configure app settings, logging, health checks, and error handling.
- [x] Implement DB access layer compatible with existing schema.
- [x] Set up auth cookie/session policies equivalent to legacy behavior.
- [x] Add observability dashboard (RequestMetricsMiddleware + /metrics admin endpoint: request counts, errors, auth failures, upload metrics, latency).

## Phase 4: Authentication and Identity Migration
- [x] Choose migration strategy:
- [x] Side-by-side auth (legacy + new), or
- [x] One-time cutover with password reset.
- [x] Model user/role mapping from `aspnet_*` tables to ASP.NET Core Identity.
- [x] Build and test migration scripts for users, roles, and key profile fields (CS/db/migrate-identity.sql + rollback-identity.sql).
- [x] Validate lockout/reset/confirm flows.
- [x] Run security review for auth/session/cookie settings (SQL injection blocklist, file upload validation, cookie hardening, security headers, email confirmation tokens).

## Phase 5: Feature-by-Feature Web Migration
- [x] Migrate public pages first (home, browse, about/contact, watch).
- [x] Migrate member area next (upload/profile/password/video management).
- [x] Migrate CMS/admin pages last.
- [x] Replace `SqlDataSource/GridView` with typed queries/services + Razor/MVC UI.
- [x] Replace `FCKeditor` integration with supported editor component.
- [x] Add compatibility redirects/routes for old URLs.
- [x] Validate each migrated slice against regression checklist before marking complete (CS/docs/regression-checklist.md — 71-item checklist covering auth, pages, member, admin, encoder, data, security, observability).

## Phase 6: Encoder Service Migration
- [x] Create .NET 10 Worker Service for encoding pipeline.
- [x] Replace Windows-only path assumptions with cross-platform path handling.
- [x] Validate ffmpeg invocation, thumbnail generation, and output naming.
- [x] Preserve DB status updates (`Pending`, `IsActive`) behavior.
- [x] Validate success/failure email paths.
- [x] Add retry, dead-letter, and idempotency safeguards (3 retries with linear backoff, dead-letter directory, skip-if-already-active idempotency check).

## Phase 7: Pre-Cutover, Cutover, and Cleanup (Release B)
- [ ] Run full end-to-end UAT in staging with production-like data.
- [ ] Execute load/performance checks for upload/watch/search/auth.
- [ ] Validate rollback plan (traffic switch-back + DB safety constraints).
- [ ] Cut over traffic to ASP.NET Core app.
- [ ] Closely monitor metrics/logs during stabilization window.
- [ ] Decommission Web Forms deployment path after signoff.
- [ ] Archive or remove VB duplicate solution tree if no longer needed.

## Release Gates

### Gate A (after Phase 1)
- [ ] Legacy app stable on .NET Framework 4.8 in production.
- [ ] No P1/P2 regressions for one full release cycle.

### Gate B (before Release B)
- [ ] 100% critical-path parity checks passed.
- [ ] Auth migration verified with rollback tested.
- [ ] Encoder service validated in staging.
- [ ] Observability and alerting in place.
- [ ] Runbook and on-call ownership confirmed.

## Risks and Mitigations
- [ ] Web Forms to ASP.NET Core rewrite risk: migrate by vertical slice, not big-bang.
- [ ] Auth compatibility risk: support dual-path auth or enforce controlled password reset.
- [ ] Legacy editor risk: replace early in staging and test content workflows.
- [ ] DB coupling risk: keep schema stable until app parity is proven.
- [ ] Deployment risk: enforce blue/green or equivalent reversible rollout.

## Ownership and Timeline
- [ ] Assign technical owner per phase.
- [ ] Add target dates for each phase.
- [ ] Track completion weekly with go/no-go review notes.

