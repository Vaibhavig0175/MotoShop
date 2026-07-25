using MotoShop.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoShop.Models
{
    public class VehicleBrand: BaseEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public int VehicleCategoryId { get; set; }

        [ForeignKey(nameof(VehicleCategoryId))]
        public VehicleCategory? VehicleCategory { get; set; }
        public ICollection<VehicleModel> VehicleModels { get; set; } = new List<VehicleModel>();
    }
}
