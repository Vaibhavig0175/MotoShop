using MotoShop.Data;
using MotoShop.Enums;
using MotoShop.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoShop.Models
{
    public class Auction: BaseEntity
    {
        public int Id { get; set; }
        [Required]
        public int VehicleId { get; set; }

        [ForeignKey(nameof(VehicleId))]
        public Vehicle Vehicle { get; set; } = null!;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [Required]
        public decimal StartingPrice { get; set; }

        public decimal ReservePrice { get; set; }

        public decimal CurrentBid { get; set; }

        public bool IsClosed { get; set; }

        public string? WinnerId { get; set; }
        public AuctionStatus Status { get; set; } = AuctionStatus.Draft;

        [ForeignKey(nameof(WinnerId))]
        public ApplicationUser? Winner { get; set; }
        public decimal? WinningBidAmount { get; set; }

        public DateTime? ClosedOn { get; set; }
        public ICollection<Bid> Bids { get; set; } = new List<Bid>();

    }
}
