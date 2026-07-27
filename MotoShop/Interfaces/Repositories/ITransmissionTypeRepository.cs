using MotoShop.Models;

namespace MotoShop.Interfaces.Repositories
{
    public interface ITransmissionTypeRepository
    {
        Task<List<TransmissionType>> GetAllAsync();
        Task<TransmissionType?> GetByIdAsync(int id);
        Task AddAsync(TransmissionType transmissionType);
        Task UpdateAsync(TransmissionType transmissionType);
        Task DeleteAsync(int id);
    }
}
