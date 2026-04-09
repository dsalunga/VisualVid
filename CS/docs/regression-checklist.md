# VisualVid Regression Validation Checklist

This checklist maps every legacy feature to its new ASP.NET Core equivalent.
All items must pass before promoting to production.

## Authentication & Authorization

| # | Feature | Legacy (Web Forms) | New (ASP.NET Core) | Status |
|---|---------|-------|-----|--------|
| 1 | Login | Login.aspx | /Account/Login | [ ] |
| 2 | Logout | POST to Login.aspx | POST /Account/Logout | [ ] |
| 3 | Registration | Register.aspx | /Account/Register | [ ] |
| 4 | Email confirmation | Confirm.aspx?user=&confirmCode= | /Account/Confirm?userId=&token= | [ ] |
| 5 | Forgot password | Forgot.aspx | /Account/ForgotPassword | [ ] |
| 6 | Change password | Change.aspx | /Account/ChangePassword | [ ] |
| 7 | Role-based access (Admin) | aspnet_Membership Roles | ASP.NET Core Identity Roles | [ ] |
| 8 | Role-based access (Members) | aspnet_Membership Roles | ASP.NET Core Identity Roles | [ ] |
| 9 | FormsAuth cookie | FormsAuthentication | Identity Cookie (HttpOnly, Secure, SameSite=Lax) | [ ] |
| 10 | Lockout on failed attempts | aspnet_Membership | Identity lockout (enabled) | [ ] |

## Public Pages

| # | Feature | Legacy URL | New URL | Status |
|---|---------|-----------|---------|--------|
| 11 | Home page | Default.aspx | / | [ ] |
| 12 | Browse videos | Browse.aspx | /Browse | [ ] |
| 13 | Watch video | Watch.aspx?videoId= | /Video/Watch?videoId= | [ ] |
| 14 | Legacy Watch URL | Watch.aspx?videoId= | Watch.aspx (compat route) | [ ] |
| 15 | Search | Results.aspx?q= | /Search?q= | [ ] |
| 16 | Legacy Search URL | Results.aspx?q= | Results.aspx (compat route) | [ ] |
| 17 | User profile | Profile.aspx?userId= | /Profile?userId= | [ ] |
| 18 | Legacy Profile URL | Profile.aspx?userId= | Profile.aspx (compat route) | [ ] |
| 19 | About page | About.aspx | /Home/About | [ ] |
| 20 | Contact page | Contact.aspx | /Home/Contact | [ ] |
| 21 | Privacy page | Privacy.aspx | /Home/Privacy | [ ] |
| 22 | Terms page | Terms.aspx | /Home/Terms | [ ] |
| 23 | Sitemap page | Sitemap.aspx | /Home/Sitemap | [ ] |
| 24 | View increment | Watch.aspx (code-behind) | VideoController.Watch (auto) | [ ] |
| 25 | Comments on watch page | Watch.aspx (GridView) | Watch view (Razor) | [ ] |
| 26 | Add comment | Watch.aspx POST | POST /Video/AddComment | [ ] |

## Member Area

| # | Feature | Legacy URL | New URL | Status |
|---|---------|-----------|---------|--------|
| 27 | Dashboard | Member/Default.aspx | /Member/Dashboard | [ ] |
| 28 | Upload video | Member/Upload.aspx | /Member/Video/Upload | [ ] |
| 29 | Video pending flow | Upload → DB (Pending=1) → Encoder | Upload → DB (Pending=1) → Encoder | [ ] |
| 30 | Upload complete page | Member/UploadComplete.aspx | /Member/Video/UploadComplete | [ ] |
| 31 | Edit video | Member/Edit.aspx?videoId= | /Member/Video/Edit?videoId= | [ ] |
| 32 | Delete video | Member/Default.aspx (action) | /Member/Video/Delete | [ ] |
| 33 | Edit profile | Member/Profile.aspx | /Member/Profile/Edit | [ ] |
| 34 | Profile photo upload | Member/Profile.aspx (FileUpload) | /Member/Profile/Edit (IFormFile) | [ ] |
| 35 | Change password (member) | Member/Password.aspx | /Member/Profile/Password | [ ] |

## Admin Area

| # | Feature | Legacy URL | New URL | Status |
|---|---------|-----------|---------|--------|
| 36 | Admin dashboard | _CMS/ pages | /Admin/Admin | [ ] |
| 37 | Manage members | CMS member management | /Admin/Admin/Members | [ ] |
| 38 | Manage videos | CMS video management | /Admin/Admin/Videos | [ ] |
| 39 | Query tool | CMS query tool | /Admin/Admin/Query | [ ] |
| 40 | Admin-only access | Role check | [Authorize(Roles = "Administrators")] | [ ] |

## Video Encoding

| # | Feature | Legacy | New | Status |
|---|---------|-------|-----|--------|
| 41 | Polling for pending videos | Windows console app loop | BackgroundService loop | [ ] |
| 42 | ffmpeg thumbnail | ffmpeg -i ... -vframes 1 | Same (async) | [ ] |
| 43 | ffmpeg encoding | FLV encoding (VP6) | H.264/MP4 encoding | [ ] |
| 44 | DB update on success | Pending=0, IsActive=1 | Pending=0, IsActive=1 (idempotent) | [ ] |
| 45 | Success email | SmtpClient with HTML body | Same (async) | [ ] |
| 46 | Failure handling | Delete from DB | Mark failed + dead-letter directory | [ ] |
| 47 | Retry on failure | None | 3 retries with linear backoff | [ ] |
| 48 | Idempotency | None | Skip if output exists and DB active | [ ] |

## Data Layer

| # | Feature | Legacy | New | Status |
|---|---------|-------|-----|--------|
| 49 | Videos CRUD | Stored procedures + SqlHelper | EF Core + VideoService | [ ] |
| 50 | Members CRUD | Stored procedures + SqlHelper | EF Core + MemberService | [ ] |
| 51 | Comments CRUD | Stored procedures | EF Core + VideoService | [ ] |
| 52 | Categories listing | Stored procedure | EF Core + MemberService | [ ] |
| 53 | Countries listing | Direct SQL | EF Core + MemberService | [ ] |
| 54 | Search by tags | SqlDataSource | EF Core LINQ Contains | [ ] |
| 55 | Sort by views/date | SqlDataSource | EF Core OrderByDescending | [ ] |

## Security

| # | Feature | Check | Status |
|---|---------|-------|--------|
| 56 | CSRF protection | All POST actions have [ValidateAntiForgeryToken] | [ ] |
| 57 | SQL injection prevention | Admin Query uses keyword blocklist | [ ] |
| 58 | File upload validation | Video: extension whitelist; Photo: extension + size | [ ] |
| 59 | Email confirmation | Uses Identity's GenerateEmailConfirmationTokenAsync | [ ] |
| 60 | Cookie security | HttpOnly=true, Secure=Always, SameSite=Lax | [ ] |
| 61 | Security headers | X-Content-Type-Options, X-Frame-Options, Referrer-Policy, Permissions-Policy | [ ] |
| 62 | Open redirect prevention | Url.IsLocalUrl on returnUrl in Login | [ ] |
| 63 | Password hashing | ASP.NET Core Identity PBKDF2-HMAC-SHA256 | [ ] |
| 64 | Admin endpoint protection | [Authorize(Roles = "Administrators")] on AdminController | [ ] |
| 65 | Metrics endpoint | Requires Admin role | [ ] |

## Observability

| # | Feature | Implementation | Status |
|---|---------|---------------|--------|
| 66 | Request metrics | RequestMetricsMiddleware (total, errors, latency) | [ ] |
| 67 | Auth failure tracking | Middleware counts 401/403 responses | [ ] |
| 68 | Upload failure tracking | Middleware counts failed upload POSTs | [ ] |
| 69 | Health check endpoint | /health (ASP.NET Core built-in) | [ ] |
| 70 | Admin metrics endpoint | /metrics (admin-only JSON) | [ ] |
| 71 | Structured logging | Serilog-compatible {Property} format | [ ] |
