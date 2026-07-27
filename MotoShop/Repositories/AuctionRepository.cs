using MotoShop.Data;
using MotoShop.Enums;
using MotoShop.Interfaces.Repositories;
using MotoShop.Models;
using Microsoft.EntityFrameworkCore;

namespace MotoShop.Repositories
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly ApplicationDbContext _context;

        public AuctionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Auction>> GetAllAsync()
        {
            return await _context.Auctions
                .Include(x => x.Vehicle)
                    .ThenInclude(v => v.VehicleBrand)
                .Include(x => x.Vehicle)
                    .ThenInclude(v => v.VehicleModel)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
        }

        public async Task<List<Auction>> GetActiveAsync()
        {
            var now = DateTime.UtcNow;

            return await _context.Auctions
                .Include(x => x.Vehicle)
                .Where(x =>
                    x.StartDate <= now &&
                    x.EndDate >= now &&
                    !x.IsClosed)
                .ToListAsync();
        }

        public async Task<Auction?> GetByIdAsync(int id)
        {
            return await _context.Auctions
                .Include(x => x.Vehicle)
                    .ThenInclude(v => v.VehicleBrand)
                .Include(x => x.Vehicle)
                    .ThenInclude(v => v.VehicleModel)
                .Include(x => x.Vehicle)
                    .ThenInclude(v => v.Images)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(Auction auction)
        {
            _context.Auctions.Add(auction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Auction auction)
        {
            _context.Auctions.Update(auction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var auction = await _context.Auctions.FindAsync(id);

            if (auction == null)
                return;

            _context.Auctions.Remove(auction);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsForVehicleAsync(int vehicleId)
        {
            return await _context.Auctions
                .AnyAsync(x => x.VehicleId == vehicleId);
        }

        public async Task<List<Vehicle>> GetApprovedVehiclesAsync()
        {
            return await _context.Vehicles
                .Include(x => x.VehicleBrand)
                .Include(x => x.VehicleModel)
                .Where(x => x.Status == VehicleStatus.Approved)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
        }
        public async Task CloseExpiredAuctionsAsync()
        {
            var auctions = await _context.Auctions
                .Include(x => x.Bids)
                .Where(x => !x.IsClosed &&
                            x.EndDate <= DateTime.UtcNow)
                .ToListAsync();

            foreach (var auction in auctions)
            {
                auction.IsClosed = true;

                if (auction.Bids.Any())
                {
                    var winner = auction.Bids
                        .OrderByDescending(x => x.Amount)
                        .First();

                    winner.IsWinningBid = true;

                    auction.WinnerId = winner.BuyerId;

                    auction.CurrentBid = winner.Amount;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
