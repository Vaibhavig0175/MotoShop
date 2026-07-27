using MotoShop.Models.Common;

namespace MotoShop.Models
{
    public class VehicleImage: BaseEntity
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public bool IsPrimary { get; set; }

        public virtual Vehicle Vehicle { get; set; } = null!;
    }
}
