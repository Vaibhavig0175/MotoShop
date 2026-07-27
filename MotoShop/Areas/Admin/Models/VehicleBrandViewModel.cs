using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MotoShop.Areas.Admin.Models
{
    public class VehicleBrandViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int VehicleCategoryId { get; set; }

        public List<SelectListItem> Categories { get; set; } = new();
    }
}
