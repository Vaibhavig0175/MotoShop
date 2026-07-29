using MotoShop.Enums;
using MotoShop.Models;

namespace MotoShop.Services
{
    public static class AuctionStatusService
    {
        public static AuctionStatus GetStatus(Auction auction)
        {
            if (auction.Status == AuctionStatus.Cancelled)
                return AuctionStatus.Cancelled;

            if (DateTime.Now < auction.StartTime)
                return AuctionStatus.Upcoming;

            if (DateTime.Now >= auction.StartTime &&
                DateTime.Now <= auction.EndTime)
                return AuctionStatus.Live;

            return AuctionStatus.Closed;
        }
    }
}
