using Microsoft.AspNetCore.Identity;
using MotoShop.Models;
using System.ComponentModel.DataAnnotations;
namespace MotoShop.Data;
// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public SellerProfile? SellerProfile { get; set; }
}
