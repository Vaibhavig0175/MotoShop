using MotoShop.Models;

namespace MotoShop.Interfaces.Services
{
    public interface IVehicleService
    {
        Task<List<Vehicle>> GetAllAsync();

        Task<List<Vehicle>> GetSellerVehiclesAsync(string sellerId);

        Task<List<Vehicle>> GetPendingVehiclesAsync();

        Task<List<Vehicle>> GetApprovedVehiclesAsync();

        Task<Vehicle?> GetByIdAsync(int id);

        Task AddAsync(Vehicle vehicle);

        Task UpdateAsync(Vehicle vehicle);

        Task DeleteAsync(int id);

        Task ApproveVehicleAsync(int id);

        Task RejectVehicleAsync(int id, string reason);
        Task UploadImagesAsync(int vehicleId, List<IFormFile> files);

        Task<List<VehicleImage>> GetImagesAsync(int vehicleId);

        Task<Vehicle?> GetByIdWithDetailsAsync(int id);

        Task ApproveAsync(int id);

        Task RejectAsync(int id, string reason);
    }
}
