using MotoShop.Models;

namespace MotoShop.Interfaces.Repositories
{
    public interface IVehicleColorRepository
    {
        Task<List<VehicleColor>> GetAllAsync();

        Task<VehicleColor?> GetByIdAsync(int id);

        Task AddAsync(VehicleColor vehicleColor);

        Task UpdateAsync(VehicleColor vehicleColor);

        Task DeleteAsync(int id);
    }
}
