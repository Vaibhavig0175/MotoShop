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
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleCategory> VehicleCategories { get; set; }
        public DbSet<VehicleBrand> VehicleBrands { get; set; } = default!;
        public DbSet<VehicleModel> VehicleModels { get; set; } = default!;
        public DbSet<FuelType> FuelTypes { get; set; } = default!;
        public DbSet<TransmissionType> TransmissionTypes { get; set; } = default!;
        public DbSet<VehicleColor> VehicleColors { get; set; } = default!;
        public DbSet<VehicleImage> VehicleImages { get; set; } = default!;
        public DbSet<Auction> Auctions { get; set; } = default!;
        public DbSet<Bid> Bids { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<SellerProfile>()
                .HasOne(s => s.User)
                .WithOne(u => u.SellerProfile)
                .HasForeignKey<SellerProfile>(s => s.UserId);

            builder.Entity<Vehicle>()
                    .Property(x => x.StartingPrice)
                    .HasPrecision(18, 2);

            builder.Entity<Vehicle>()
                .Property(x => x.ReservePrice)
                .HasPrecision(18, 2);

            builder.Entity<Vehicle>()
               .HasOne(v => v.VehicleCategory)
               .WithMany()
               .HasForeignKey(v => v.VehicleCategoryId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Vehicle>()
                .HasOne(v => v.VehicleBrand)
                .WithMany()
                .HasForeignKey(v => v.VehicleBrandId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Vehicle>()
                .HasOne(v => v.VehicleModel)
                .WithMany()
                .HasForeignKey(v => v.VehicleModelId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Vehicle>()
                .HasOne(v => v.FuelType)
                .WithMany()
                .HasForeignKey(v => v.FuelTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Vehicle>()
                .HasOne(v => v.Seller)
                .WithMany()
                .HasForeignKey(v => v.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<VehicleImage>()
                .HasOne(x => x.Vehicle)
                .WithMany(x => x.Images)
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Auction>()
            .HasOne(a => a.Vehicle)
            .WithOne(v => v.Auction)
            .HasForeignKey<Auction>(a => a.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Bid>()
                .HasOne(x => x.Auction)
                .WithMany(x => x.Bids)
                .HasForeignKey(x => x.AuctionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Bid>()
                .HasOne(x => x.Buyer)
                .WithMany(x => x.Bids)
                .HasForeignKey(x => x.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
