using MotoShop.Models;

namespace MotoShop.Interfaces.Services
{
    public interface IAuctionService
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
