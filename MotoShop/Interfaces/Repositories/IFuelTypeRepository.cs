using MotoShop.Models;

namespace MotoShop.Interfaces.Repositories
{
    public interface IFuelTypeRepository
    {
        Task<List<FuelType>> GetAllAsync();

        Task<FuelType?> GetByIdAsync(int id);

        Task AddAsync(FuelType fuelType);

        Task UpdateAsync(FuelType fuelType);

        Task DeleteAsync(int id);
    }
}
