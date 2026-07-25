using MotoShop.Models;

namespace MotoShop.Interfaces.Services
{
    public interface IVehicleModelService
    {
        Task<List<VehicleModel>> GetAllAsync();

        Task<VehicleModel?> GetByIdAsync(int id);

        Task AddAsync(VehicleModel model);

        Task UpdateAsync(VehicleModel model);

        Task DeleteAsync(int id);
    }
}
