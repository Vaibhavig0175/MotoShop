namespace MotoShop.Areas.Admin.Models
{
    public class AuctionDashboardViewModel
    {
        public int TotalAuctions { get; set; }

        public int UpcomingAuctions { get; set; }

        public int LiveAuctions { get; set; }

        public int ClosedAuctions { get; set; }

        public int CancelledAuctions { get; set; }

        public List<AuctionListViewModel> Auctions { get; set; } = new();
    }
}
