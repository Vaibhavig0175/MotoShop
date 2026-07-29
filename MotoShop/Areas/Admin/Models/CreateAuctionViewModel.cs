using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MotoShop.Areas.Admin.Models
{
    public class CreateAuctionViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Vehicle")]
        public int VehicleId { get; set; }

        [Required]
        [Display(Name = "Starting Price")]
        public decimal StartingPrice { get; set; }

        [Required]
        [Display(Name = "Reserve Price")]
        public decimal ReservePrice { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }

        public IEnumerable<SelectListItem> Vehicles { get; set; }
            = new List<SelectListItem>();
    }
}
