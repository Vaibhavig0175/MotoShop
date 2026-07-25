using MotoShop.Models;

namespace MotoShop.Interfaces.Services
{
    public interface IFuelTypeService
    {
        Task<List<FuelType>> GetAllAsync();

        Task<FuelType?> GetByIdAsync(int id);

        Task AddAsync(FuelType fuelType);

        Task UpdateAsync(FuelType fuelType);

        Task DeleteAsync(int id);
    }
}
