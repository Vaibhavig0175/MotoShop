using MotoShop.Interfaces.Repositories;
using MotoShop.Interfaces.Services;
using MotoShop.Models;

namespace MotoShop.Services
{
    public class FuelTypeService: IFuelTypeService
    {
        private readonly IFuelTypeRepository _repository;

        public FuelTypeService(IFuelTypeRepository repository)
        {
            _repository = repository;
        }

        public Task<List<FuelType>> GetAllAsync()
            => _repository.GetAllAsync();

        public Task<FuelType?> GetByIdAsync(int id)
            => _repository.GetByIdAsync(id);

        public Task AddAsync(FuelType fuelType)
            => _repository.AddAsync(fuelType);

        public Task UpdateAsync(FuelType fuelType)
            => _repository.UpdateAsync(fuelType);

        public Task DeleteAsync(int id)
            => _repository.DeleteAsync(id);
    }
}
