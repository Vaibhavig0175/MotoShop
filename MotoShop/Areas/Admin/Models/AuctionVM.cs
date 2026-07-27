using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MotoShop.Areas.Admin.Models
{
    public class AuctionVM
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Vehicle")]
        public int VehicleId { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Required]
        [Range(1, 999999999)]
        public decimal StartingPrice { get; set; }

        public decimal? ReservePrice { get; set; }

        public IEnumerable<SelectListItem> Vehicles { get; set; }
            = Enumerable.Empty<SelectListItem>();
    }
}
