using MotoShop.Data;
using System.ComponentModel.DataAnnotations;

namespace MotoShop.Models
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string SellerId { get; set; } = string.Empty;

        public ApplicationUser Seller { get; set; } = null!;
    }
}