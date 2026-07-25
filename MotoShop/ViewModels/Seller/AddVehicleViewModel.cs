using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MotoShop.ViewModels.Seller
{
    public class AddVehicleViewModel
    {
        [Required]
        [Display(Name = "Vehicle Title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        public int VehicleCategoryId { get; set; }

        [Required]
        public int VehicleBrandId { get; set; }

        [Required]
        public int VehicleModelId { get; set; }

        [Required]
        public int FuelTypeId { get; set; }

        [Required]
        public string Transmission { get; set; } = string.Empty;

        [Required]
        public string Color { get; set; } = string.Empty;

        [Required]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Required]
        public int ManufacturingYear { get; set; }

        [Required]
        public int KilometersDriven { get; set; }

        [Required]
        public int Owners { get; set; }

        [Required]
        public decimal StartingPrice { get; set; }

        public decimal? ReservePrice { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        public List<IFormFile>? Images { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> Brands { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> Models { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> FuelTypes { get; set; }
            = Enumerable.Empty<SelectListItem>();
    }
}
