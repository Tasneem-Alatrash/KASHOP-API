using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KASHOP.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
namespace KASHOP.DAL
{
  public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
  {

    public DbSet<Category> Categories { get; set; }
    public DbSet<CategoryTrinslation> CategoryTrinslations { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductTranslation> ProductTranslations { get; set; }
    private readonly IHttpContextAccessor _iHTTPContextAccessorz;
    private readonly object _httpContextAccessor;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor iHTTPContextAccessorz)
  : base(options)
    {
      _iHTTPContextAccessorz = iHTTPContextAccessorz;
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
      builder.Entity<ApplicationUser>().ToTable("Users");
      builder.Entity<IdentityRole>().ToTable("Roles");
      builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
      builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
      builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
      builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
      builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
      builder.Entity<Category>().HasOne(c => c.User).WithMany().HasForeignKey(c => c.CreatedBy).OnDelete(DeleteBehavior.NoAction);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
      if (_iHTTPContextAccessorz.HttpContext != null)
      {
        var entries = ChangeTracker.Entries<BaseModel>();
        var CurrentserId = _iHTTPContextAccessorz.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        foreach (var entityEntry in entries)
        {
          if (entityEntry.State == EntityState.Added)
          {
            entityEntry.Property(x => x.CreatedBy).CurrentValue = CurrentserId;
            entityEntry.Property(x => x.CreatedAt).CurrentValue = DateTime.UtcNow;
          }
          else if (entityEntry.State == EntityState.Modified)
          {
            entityEntry.Property(x => x.UpdatedBy).CurrentValue = CurrentserId;
            entityEntry.Property(x => x.UpdatedAt).CurrentValue = DateTime.UtcNow;
          }
        }
      }

      return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
    public override int SaveChanges()
    {
      var entries = ChangeTracker.Entries<BaseModel>();

      foreach (var entityEntry in entries)
      {
        var CurrentserId = _iHTTPContextAccessorz.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (entityEntry.State == EntityState.Added)
        {
          entityEntry.Property(x => x.CreatedBy).CurrentValue = CurrentserId;
          entityEntry.Property(x => x.CreatedAt).CurrentValue = DateTime.UtcNow;
        }
        else if (entityEntry.State == EntityState.Modified)
        {
          entityEntry.Property(x => x.UpdatedBy).CurrentValue = CurrentserId;
          entityEntry.Property(x => x.UpdatedAt).CurrentValue = DateTime.UtcNow;
        }
      }
      return base.SaveChanges();
    }
  }
}