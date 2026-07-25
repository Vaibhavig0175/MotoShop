using MotoShop.Interfaces.Repositories;
using MotoShop.Interfaces.Services;
using MotoShop.Models;

namespace MotoShop.Services
{
    public class TransmissionTypeService: ITransmissionTypeService
    {
        private readonly ITransmissionTypeRepository _repository;

        public TransmissionTypeService(ITransmissionTypeRepository repository)
        {
            _repository = repository;
        }

        public Task<List<TransmissionType>> GetAllAsync() => _repository.GetAllAsync();

        public Task<TransmissionType?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(TransmissionType transmissionType) => _repository.AddAsync(transmissionType);

        public Task UpdateAsync(TransmissionType transmissionType) => _repository.UpdateAsync(transmissionType);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}
