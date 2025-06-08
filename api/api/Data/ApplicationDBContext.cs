using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

public class ApplicationDBContext : IdentityDbContext<AppUser>
{
    public ApplicationDBContext(DbContextOptions dbContextOptions)
    : base(dbContextOptions)
    {
    }
    
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Portfolio> Portfolios { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // sets a composite key using AppUserId and StockId
        builder.Entity<Portfolio>(portfolio => portfolio.HasKey(p => new { p.AppUserId, p.StockId }));

        builder.Entity<Portfolio>().HasOne(p => p.AppUser).WithMany(u => u.Portfolios).HasForeignKey(p => p.AppUserId);
        
        builder.Entity<Portfolio>().HasOne(p => p.Stock).WithMany(u => u.Portfolios).HasForeignKey(p => p.StockId);

        List<IdentityRole> roles = new()
        {
            new IdentityRole
            {
                Id = "1",
                Name = "Admin",
                NormalizedName = "ADMIN"
            },

            new IdentityRole
            {
                Id = "2",
                Name = "User",
                NormalizedName = "USER"
            }
        };
        builder.Entity<IdentityRole>().HasData(roles);
    }
}