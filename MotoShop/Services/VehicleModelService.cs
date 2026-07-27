using MotoShop.Interfaces.Repositories;
using MotoShop.Interfaces.Services;
using MotoShop.Models;

namespace MotoShop.Services
{
    public class VehicleModelService: IVehicleModelService
    {
        private readonly IVehicleModelRepository _repository;

        public VehicleModelService(IVehicleModelRepository repository)
        {
            _repository = repository;
        }

        public Task<List<VehicleModel>> GetAllAsync()
            => _repository.GetAllAsync();

        public Task<VehicleModel?> GetByIdAsync(int id)
            => _repository.GetByIdAsync(id);

        public Task AddAsync(VehicleModel model)
            => _repository.AddAsync(model);

        public Task UpdateAsync(VehicleModel model)
            => _repository.UpdateAsync(model);

        public Task DeleteAsync(int id)
            => _repository.DeleteAsync(id);
    }
}
