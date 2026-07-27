using Microsoft.EntityFrameworkCore;
using MotoShop.Data;
using MotoShop.Interfaces.Repositories;
using MotoShop.Models;

namespace MotoShop.Repositories
{
    public class BidRepository: IBidRepository
    {
        private readonly ApplicationDbContext _context;

        public BidRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Bid>> GetAuctionBidsAsync(int auctionId)
        {
            return await _context.Bids
                .Include(x => x.Buyer)
                .Where(x => x.AuctionId == auctionId)
                .OrderByDescending(x => x.Amount)
                .ToListAsync();
        }

        public async Task<Bid?> GetHighestBidAsync(int auctionId)
        {
            return await _context.Bids
                .OrderByDescending(x => x.Amount)
                .FirstOrDefaultAsync(x => x.AuctionId == auctionId);
        }

        public async Task AddAsync(Bid bid)
        {
            _context.Bids.Add(bid);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasUserBidAsync(int auctionId, string buyerId)
        {
            return await _context.Bids
                .AnyAsync(x => x.AuctionId == auctionId &&
                               x.BuyerId == buyerId);
        }

        public async Task<List<Bid>> GetBuyerBidsAsync(string buyerId)
        {
            return await _context.Bids
                .Include(x => x.Auction)
                    .ThenInclude(a => a.Vehicle)
                        .ThenInclude(v => v.VehicleBrand)

                .Include(x => x.Auction)
                    .ThenInclude(a => a.Vehicle)
                        .ThenInclude(v => v.VehicleModel)

                .Include(x => x.Auction)
                    .ThenInclude(a => a.Vehicle)
                        .ThenInclude(v => v.Images)

                .Where(x => x.BuyerId == buyerId)

                .OrderByDescending(x => x.CreatedOn)

                .ToListAsync();
        }
        public async Task<Bid?> GetWinningBidAsync(int auctionId)
        {
            return await _context.Bids
                .Include(x => x.Buyer)
                .Where(x => x.AuctionId == auctionId)
                .OrderByDescending(x => x.Amount)
                .FirstOrDefaultAsync();
        }
    }
}
