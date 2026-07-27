using MotoShop.Interfaces.Repositories;
using MotoShop.Interfaces.Services;
using MotoShop.Models;

namespace MotoShop.Services
{
    public class AuctionService: IAuctionService
    {
        private readonly IAuctionRepository _repository;

        public AuctionService(IAuctionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Auction>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<List<Auction>> GetActiveAsync()
        {
            return await _repository.GetActiveAsync();
        }

        public async Task<Auction?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(Auction auction)
        {
            await _repository.AddAsync(auction);
        }

        public async Task UpdateAsync(Auction auction)
        {
            await _repository.UpdateAsync(auction);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<bool> ExistsForVehicleAsync(int vehicleId)
        {
            return await _repository.ExistsForVehicleAsync(vehicleId);
        }

        public async Task<List<Vehicle>> GetApprovedVehiclesAsync()
        {
            return await _repository.GetApprovedVehiclesAsync();
        }
        public async Task CloseExpiredAuctionsAsync()
        {
            await _repository.CloseExpiredAuctionsAsync();
        }
    }
}
