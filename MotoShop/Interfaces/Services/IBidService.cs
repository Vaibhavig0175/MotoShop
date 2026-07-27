using MotoShop.Models;

namespace MotoShop.Interfaces.Services
{
    public interface IBidService
    {
        Task<List<Bid>> GetAuctionBidsAsync(int auctionId);

        Task<Bid?> GetHighestBidAsync(int auctionId);

        Task AddAsync(Bid bid);

        Task<bool> HasUserBidAsync(int auctionId, string buyerId);

        Task<List<Bid>> GetBuyerBidsAsync(string buyerId);
        Task<Bid?> GetWinningBidAsync(int auctionId);
    }
}
