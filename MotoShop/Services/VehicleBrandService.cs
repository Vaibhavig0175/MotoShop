using MotoShop.Interfaces.Repositories;
using MotoShop.Interfaces.Services;
using MotoShop.Models;

namespace MotoShop.Services
{
    public class VehicleBrandService: IVehicleBrandService
    {
        private readonly IVehicleBrandRepository _repository;

        public VehicleBrandService(IVehicleBrandRepository repository)
        {
            _repository = repository;
        }

        public Task<List<VehicleBrand>> GetAllAsync()
            => _repository.GetAllAsync();

        public Task<VehicleBrand?> GetByIdAsync(int id)
            => _repository.GetByIdAsync(id);

        public Task AddAsync(VehicleBrand brand)
            => _repository.AddAsync(brand);

        public Task UpdateAsync(VehicleBrand brand)
            => _repository.UpdateAsync(brand);

        public Task DeleteAsync(int id)
            => _repository.DeleteAsync(id);
        public Task<List<VehicleBrand>> GetByCategoryAsync(int categoryId)
    => _repository.GetByCategoryAsync(categoryId);
    }
}
