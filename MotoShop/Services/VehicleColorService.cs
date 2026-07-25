using MotoShop.Interfaces.Repositories;
using MotoShop.Interfaces.Services;
using MotoShop.Models;

namespace MotoShop.Services
{
    public class VehicleColorService: IVehicleColorService
    {
        private readonly IVehicleColorRepository _repository;

        public VehicleColorService(IVehicleColorRepository repository)
        {
            _repository = repository;
        }

        public Task<List<VehicleColor>> GetAllAsync()
            => _repository.GetAllAsync();

        public Task<VehicleColor?> GetByIdAsync(int id)
            => _repository.GetByIdAsync(id);

        public Task AddAsync(VehicleColor vehicleColor)
            => _repository.AddAsync(vehicleColor);

        public Task UpdateAsync(VehicleColor vehicleColor)
            => _repository.UpdateAsync(vehicleColor);

        public Task DeleteAsync(int id)
            => _repository.DeleteAsync(id);
    }
}
