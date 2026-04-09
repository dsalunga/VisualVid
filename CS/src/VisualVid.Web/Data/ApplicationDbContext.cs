using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VisualVid.Web.Models;

namespace VisualVid.Web.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Video> Videos => Set<Video>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Country> Countries => Set<Country>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var isPostgres = Database.ProviderName?.Contains("Npgsql", StringComparison.OrdinalIgnoreCase) == true;
        var newGuidSql = isPostgres ? "gen_random_uuid()" : "NEWID()";

        if (isPostgres)
        {
            // PostgreSQL uses standard Identity table names (no legacy aspnet_* mapping)
            builder.Entity<ApplicationUser>(b =>
            {
                b.ToTable("aspnet_users");
                b.Property(u => u.Id).HasColumnName("user_id");
                b.Property(u => u.UserName).HasColumnName("user_name").HasMaxLength(256);
                b.Property(u => u.NormalizedUserName).HasColumnName("lowered_user_name").HasMaxLength(256);
                b.Property(u => u.Email).HasMaxLength(256);
                b.Property(u => u.NormalizedEmail).HasMaxLength(256);
            });

            builder.Entity<ApplicationRole>(b =>
            {
                b.ToTable("aspnet_roles");
                b.Property(r => r.Id).HasColumnName("role_id");
                b.Property(r => r.Name).HasColumnName("role_name").HasMaxLength(256);
                b.Property(r => r.NormalizedName).HasColumnName("lowered_role_name").HasMaxLength(256);
            });

            builder.Entity<IdentityUserRole<Guid>>(b => b.ToTable("aspnet_users_in_roles"));
            builder.Entity<IdentityUserClaim<Guid>>(b => b.ToTable("aspnet_user_claims"));
            builder.Entity<IdentityUserLogin<Guid>>(b => b.ToTable("aspnet_user_logins"));
            builder.Entity<IdentityRoleClaim<Guid>>(b => b.ToTable("aspnet_role_claims"));
            builder.Entity<IdentityUserToken<Guid>>(b => b.ToTable("aspnet_user_tokens"));
        }
        else
        {
            // SQL Server: map to legacy aspnet_* table names
            builder.Entity<ApplicationUser>(b =>
            {
                b.ToTable("aspnet_Users");
                b.Property(u => u.Id).HasColumnName("UserId");
                b.Property(u => u.UserName).HasColumnName("UserName").HasMaxLength(256);
                b.Property(u => u.NormalizedUserName).HasColumnName("LoweredUserName").HasMaxLength(256);
                b.Property(u => u.Email).HasMaxLength(256);
                b.Property(u => u.NormalizedEmail).HasMaxLength(256);
            });

            builder.Entity<ApplicationRole>(b =>
            {
                b.ToTable("aspnet_Roles");
                b.Property(r => r.Id).HasColumnName("RoleId");
                b.Property(r => r.Name).HasColumnName("RoleName").HasMaxLength(256);
                b.Property(r => r.NormalizedName).HasColumnName("LoweredRoleName").HasMaxLength(256);
            });

            builder.Entity<IdentityUserRole<Guid>>(b => b.ToTable("aspnet_UsersInRoles"));
            builder.Entity<IdentityUserClaim<Guid>>(b => b.ToTable("AspNetUserClaims"));
            builder.Entity<IdentityUserLogin<Guid>>(b => b.ToTable("AspNetUserLogins"));
            builder.Entity<IdentityRoleClaim<Guid>>(b => b.ToTable("AspNetRoleClaims"));
            builder.Entity<IdentityUserToken<Guid>>(b => b.ToTable("AspNetUserTokens"));
        }

        // Videos table
        builder.Entity<Video>(b =>
        {
            b.ToTable("Videos");
            b.HasKey(v => v.VideoId);
            b.Property(v => v.VideoId).HasDefaultValueSql(newGuidSql);
            b.Property(v => v.Title).HasMaxLength(255);
            b.Property(v => v.Description).HasMaxLength(4000);
            b.Property(v => v.Tags).HasMaxLength(4000);
            b.Property(v => v.OriginalExtension).HasMaxLength(64);
            b.Property(v => v.CategoryId).HasColumnName("CategoryID");

            b.HasOne(v => v.Category)
                .WithMany(c => c.Videos)
                .HasForeignKey(v => v.CategoryId);

            b.HasOne(v => v.User)
                .WithMany()
                .HasForeignKey(v => v.UserId);
        });

        // Members table
        builder.Entity<Member>(b =>
        {
            b.ToTable("Members");
            b.HasKey(m => m.UserId);
            b.Property(m => m.UserId).HasDefaultValueSql(newGuidSql);

            b.HasOne(m => m.Country)
                .WithMany()
                .HasForeignKey(m => m.CountryCode);

            b.HasOne(m => m.User)
                .WithOne(u => u.MemberProfile)
                .HasForeignKey<Member>(m => m.UserId);
        });

        // Comments table
        builder.Entity<Comment>(b =>
        {
            b.ToTable("Comments");
            b.HasKey(c => c.CommentId);
            b.Property(c => c.CommentId).HasColumnName("CommentID").ValueGeneratedOnAdd();
            b.Property(c => c.ReplyFromId).HasColumnName("ReplyFromID");

            b.HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId);

            b.HasOne(c => c.Video)
                .WithMany(v => v.Comments)
                .HasForeignKey(c => c.VideoId);
        });

        // Categories table
        builder.Entity<Category>(b =>
        {
            b.ToTable("Categories");
            b.HasKey(c => c.CategoryId);
            b.Property(c => c.CategoryId).HasColumnName("CategoryID").ValueGeneratedOnAdd();
            b.Property(c => c.Name).HasMaxLength(255);
        });

        // Countries table
        builder.Entity<Country>(b =>
        {
            b.ToTable("Countries");
            b.HasKey(c => c.CountryCode);
            b.Property(c => c.Name).HasMaxLength(255);
        });
    }
}
