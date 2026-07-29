using Microsoft.AspNetCore.SignalR;

namespace MotoShop.Hubs
{
    public class AuctionHub: Hub
    {
        public async Task JoinAuction(string auctionId)
        {
            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                $"Auction-{auctionId}");
        }

        public async Task LeaveAuction(string auctionId)
        {
            await Groups.RemoveFromGroupAsync(
                Context.ConnectionId,
                $"Auction-{auctionId}");
        }
    }
}
