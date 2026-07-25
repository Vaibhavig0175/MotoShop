using MotoShop.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoShop.Models
{
    public class VehicleModel: BaseEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public int VehicleBrandId { get; set; }

        [ForeignKey(nameof(VehicleBrandId))]
        public VehicleBrand? VehicleBrand { get; set; }
    }
}
