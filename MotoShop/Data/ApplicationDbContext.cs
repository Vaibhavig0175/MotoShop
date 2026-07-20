using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MotoShop.Data.Migrations;
using MotoShop.Models;

namespace MotoShop.Data
{
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets will be added here
        public DbSet<SellerProfile> SellerProfiles { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<SellerProfile>()
                .HasOne(s => s.User)
                .WithOne(u => u.SellerProfile)
                .HasForeignKey<SellerProfile>(s => s.UserId);
        }
    }
}
