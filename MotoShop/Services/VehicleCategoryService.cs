using MotoShop.Interfaces.Repositories;
using MotoShop.Interfaces.Services;
using MotoShop.Models;

namespace MotoShop.Services
{
    public class VehicleCategoryService: IVehicleCategoryService
    {
        private readonly IVehicleCategoryRepository _repository;

        public VehicleCategoryService(IVehicleCategoryRepository repository)
        {
            _repository = repository;
        }

        public Task<List<VehicleCategory>> GetAllAsync()
            => _repository.GetAllAsync();

        public Task<VehicleCategory?> GetByIdAsync(int id)
            => _repository.GetByIdAsync(id);

        public Task AddAsync(VehicleCategory category)
            => _repository.AddAsync(category);

        public Task UpdateAsync(VehicleCategory category)
            => _repository.UpdateAsync(category);

        public Task DeleteAsync(int id)
            => _repository.DeleteAsync(id);
    }
}
