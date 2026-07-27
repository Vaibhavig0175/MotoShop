using MotoShop.Models;

namespace MotoShop.Interfaces.Services
{
    public interface IVehicleBrandService
    {
        Task<List<VehicleBrand>> GetAllAsync();

        Task<VehicleBrand?> GetByIdAsync(int id);

        Task AddAsync(VehicleBrand brand);

        Task UpdateAsync(VehicleBrand brand);

        Task DeleteAsync(int id);
        Task<List<VehicleBrand>> GetByCategoryAsync(int categoryId);
    }
}
