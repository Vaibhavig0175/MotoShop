using MotoShop.Models;

namespace MotoShop.Interfaces.Repositories
{
    public interface IVehicleCategoryRepository
    {
        Task<List<VehicleCategory>> GetAllAsync();

        Task<VehicleCategory?> GetByIdAsync(int id);

        Task AddAsync(VehicleCategory category);

        Task UpdateAsync(VehicleCategory category);

        Task DeleteAsync(int id);
    }
}
