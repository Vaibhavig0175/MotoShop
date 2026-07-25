using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MotoShop.Areas.Seller.Models
{
    public class AddVehicleViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Vehicle Title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Category")]
        public int VehicleCategoryId { get; set; }

        [Required]
        [Display(Name = "Brand")]
        public int VehicleBrandId { get; set; }

        [Required]
        [Display(Name = "Model")]
        public int VehicleModelId { get; set; }

        [Required]
        [Display(Name = "Fuel Type")]
        public int FuelTypeId { get; set; }

        [Required]
        [Display(Name = "Transmission")]
        public string Transmission { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Color")]
        public string Color { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Registration Number")]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Required]
        [Range(1980, 2100)]
        [Display(Name = "Manufacturing Year")]
        public int ManufacturingYear { get; set; }

        [Required]
        [Display(Name = "Kilometers Driven")]
        public int KilometersDriven { get; set; }

        [Required]
        [Display(Name = "Number of Owners")]
        public int Owners { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Starting Price")]
        public decimal StartingPrice { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Reserve Price")]
        public decimal? ReservePrice { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        // Upload Images
        [Display(Name = "Vehicle Images")]
        public List<IFormFile>? Images { get; set; }

        // Existing Images (Edit Screen)
        public List<string> ExistingImages { get; set; } = new();

        // Dropdowns
        public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> Brands { get; set; } = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> Models { get; set; } = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> FuelTypes { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}

