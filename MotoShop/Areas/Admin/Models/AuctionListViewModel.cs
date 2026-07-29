namespace MotoShop.Areas.Admin.Models
{
    public class AuctionListViewModel
    {
        public int Id { get; set; }

        public string VehicleName { get; set; } = string.Empty;

        public string SellerName { get; set; } = string.Empty;

        public decimal StartingPrice { get; set; }

        public decimal CurrentBid { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Status { get; set; } = string.Empty;

    }
}
