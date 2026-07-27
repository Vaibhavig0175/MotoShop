using MotoShop.Models;

namespace MotoShop.Interfaces.Services
{
    public interface ITransmissionTypeService
    {
        Task<List<TransmissionType>> GetAllAsync();
        Task<TransmissionType?> GetByIdAsync(int id);
        Task AddAsync(TransmissionType transmissionType);
        Task UpdateAsync(TransmissionType transmissionType);
        Task DeleteAsync(int id);
    }
}
