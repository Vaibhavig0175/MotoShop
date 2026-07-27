using MotoShop.Models;

namespace MotoShop.Interfaces.Repositories
{
    public interface IAuctionRepository
    {
        Task<List<Auction>> GetAllAsync();

        Task<List<Auction>> GetActiveAsync();

        Task<Auction?> GetByIdAsync(int id);

        Task AddAsync(Auction auction);

        Task UpdateAsync(Auction auction);

        Task DeleteAsync(int id);

        Task<bool> ExistsForVehicleAsync(int vehicleId);

        Task<List<Vehicle>> GetApprovedVehiclesAsync(); 
        Task CloseExpiredAuctionsAsync();
    }
}
