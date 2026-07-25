using MotoShop.Data;
using MotoShop.Enums;
using MotoShop.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace MotoShop.Models
{
    public class Vehicle : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public string SellerId { get; set; } = string.Empty;

        public ApplicationUser Seller { get; set; } = null!;

        [Required]
        public int VehicleCategoryId { get; set; }

        public VehicleCategory VehicleCategory { get; set; } = null!;

        [Required]
        public int VehicleBrandId { get; set; }

        public VehicleBrand VehicleBrand { get; set; } = null!;

        [Required]
        public int VehicleModelId { get; set; }

        public VehicleModel VehicleModel { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string RegistrationNumber { get; set; } = string.Empty;

        public int ManufacturingYear { get; set; }

        public int FuelTypeId { get; set; }

        public FuelType FuelType { get; set; } = null!;

        public string Transmission { get; set; } = string.Empty;

        public string Color { get; set; } = string.Empty;

        public int KilometersDriven { get; set; }

        public int Owners { get; set; }

        public decimal StartingPrice { get; set; }

        public decimal? ReservePrice { get; set; }

        public VehicleStatus Status { get; set; }
        = VehicleStatus.Draft;

        public string? RejectionReason { get; set; }
        public virtual ICollection<VehicleImage> Images { get; set; } = new List<VehicleImage>();
        public Auction? Auction { get; set; }

    }
}