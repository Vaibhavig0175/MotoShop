using MotoShop.Data;
using MotoShop.Enums;
using System.ComponentModel.DataAnnotations;

namespace MotoShop.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Required]
        public int AuctionId { get; set; }

        public Auction Auction { get; set; } = null!;

        [Required]
        public string BuyerId { get; set; } = string.Empty;

        public ApplicationUser Buyer { get; set; } = null!;

        [Required]
        public decimal Amount { get; set; }

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public string? TransactionId { get; set; }

        public DateTime? PaymentDate { get; set; }

        public string? Remarks { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public DateTime? UpdatedOn { get; set; }

        public bool IsSettled { get; set; }

        public DateTime? SettlementDate { get; set; }

        public string? SettledById { get; set; }

        public ApplicationUser? SettledBy { get; set; }
    }
}
