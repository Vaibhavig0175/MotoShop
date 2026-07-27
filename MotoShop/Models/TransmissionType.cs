using MotoShop.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace MotoShop.Models
{
    public class TransmissionType: BaseEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }
    }
}
