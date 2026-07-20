using MotoShop.Data;
using System.ComponentModel.DataAnnotations;

namespace MotoShop.Models
{
    public class SellerProfile
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public ApplicationUser User { get; set; } = null!;

        [Required]
        [StringLength(150)]
        public string BusinessName { get; set; } = string.Empty;

        [StringLength(15)]
        public string? GstNumber { get; set; }

        [Required]
        [StringLength(250)]
        public string BusinessAddress { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string State { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string Pincode { get; set; } = string.Empty;

        public bool IsVerified { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
