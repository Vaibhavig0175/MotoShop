using MotoShop.Models;

namespace MotoShop.Interfaces.Repositories
{
    public interface IVehicleRepository
    {
        Task<List<Vehicle>> GetAllAsync();

        Task<List<Vehicle>> GetSellerVehiclesAsync(string sellerId);

        Task<List<Vehicle>> GetPendingVehiclesAsync();

        Task<List<Vehicle>> GetApprovedVehiclesAsync();

        Task<Vehicle?> GetByIdAsync(int id);

        Task AddAsync(Vehicle vehicle);

        Task UpdateAsync(Vehicle vehicle);

        Task DeleteAsync(int id);
        Task AddImageAsync(VehicleImage image);

        Task<List<VehicleImage>> GetImagesAsync(int vehicleId);

        Task<Vehicle?> GetByIdWithDetailsAsync(int id);

        Task ApproveAsync(int id);

        Task RejectAsync(int id, string reason);
    }
}
