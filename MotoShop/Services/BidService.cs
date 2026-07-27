using MotoShop.Interfaces.Repositories;
using MotoShop.Interfaces.Services;
using MotoShop.Models;

namespace MotoShop.Services
{
    public class BidService: IBidService
    {
        private readonly IBidRepository _repository;

        public BidService(IBidRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Bid>> GetAuctionBidsAsync(int auctionId)
        {
            return await _repository.GetAuctionBidsAsync(auctionId);
        }

        public async Task<Bid?> GetHighestBidAsync(int auctionId)
        {
            return await _repository.GetHighestBidAsync(auctionId);
        }

        public async Task AddAsync(Bid bid)
        {
            await _repository.AddAsync(bid);
        }

        public async Task<bool> HasUserBidAsync(int auctionId, string buyerId)
        {
            return await _repository.HasUserBidAsync(auctionId, buyerId);
        }

        public async Task<List<Bid>> GetBuyerBidsAsync(string buyerId)
        {
            return await _repository.GetBuyerBidsAsync(buyerId);
        }
        public async Task<Bid?> GetWinningBidAsync(int auctionId)
        {
            return await _repository.GetWinningBidAsync(auctionId);
        }
    }
}
