using MotoShop.Models;

namespace MotoShop.Interfaces.Services
{
    public interface IVehicleColorService
    {
        Task<List<VehicleColor>> GetAllAsync();

        Task<VehicleColor?> GetByIdAsync(int id);

        Task AddAsync(VehicleColor vehicleColor);

        Task UpdateAsync(VehicleColor vehicleColor);

        Task DeleteAsync(int id);
    }
}
