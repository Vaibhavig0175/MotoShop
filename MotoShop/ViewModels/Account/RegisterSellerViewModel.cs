using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MotoShop.ViewModels.Account
{
    public class RegisterSellerViewModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Business Name")]
        public string BusinessName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Business Address")]
        public string BusinessAddress { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        // Selected state
        [Required]
        [Display(Name = "State")]
        public string State { get; set; } = string.Empty;

        // Dropdown list
        public IEnumerable<SelectListItem> States { get; set; }
            = Enumerable.Empty<SelectListItem>();

        [Required]
        [Display(Name = "PIN Code")]
        public string Pincode { get; set; } = string.Empty;

        [Display(Name = "GST Number")]
        public string? GstNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
