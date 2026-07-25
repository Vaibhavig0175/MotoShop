using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MotoShop.Areas.Admin.Models
{
    public class VehicleModelViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public int VehicleBrandId { get; set; }

        public List<SelectListItem> Brands { get; set; }
            = new();
    }
}
