using MotoShop.Models;

namespace MotoShop.Interfaces.Services
{
    public interface IVehicleCategoryService
    {
        Task<List<VehicleCategory>> GetAllAsync();

        Task<VehicleCategory?> GetByIdAsync(int id);

        Task AddAsync(VehicleCategory category);

        Task UpdateAsync(VehicleCategory category);

        Task DeleteAsync(int id);
    }
}
