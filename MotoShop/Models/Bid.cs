using MotoShop.Data;
using MotoShop.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoShop.Models
{
    public class Bid: BaseEntity
    {
        public int Id { get; set; }

        public int AuctionId { get; set; }

        [ForeignKey(nameof(AuctionId))]
        public Auction Auction { get; set; } = null!;

        public string BuyerId { get; set; } = string.Empty;

        [ForeignKey(nameof(BuyerId))]
        public ApplicationUser Buyer { get; set; } = null!;

        [Required]
        [Range(1, 999999999)]
        public decimal Amount { get; set; }

        public bool IsWinningBid { get; set; } = false;

        public DateTime BidTime { get; set; } = DateTime.UtcNow;
    }
}
