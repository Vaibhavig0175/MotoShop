using MotoShop.Data;
using MotoShop.Models;
using Microsoft.EntityFrameworkCore;
using MotoShop.Interfaces.Repositories;
namespace MotoShop.Repositories
{
    public class TransmissionTypeRepository: ITransmissionTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public TransmissionTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TransmissionType>> GetAllAsync()
        {
            return await _context.TransmissionTypes
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<TransmissionType?> GetByIdAsync(int id)
        {
            return await _context.TransmissionTypes.FindAsync(id);
        }

        public async Task AddAsync(TransmissionType transmissionType)
        {
            await _context.TransmissionTypes.AddAsync(transmissionType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TransmissionType transmissionType)
        {
            _context.TransmissionTypes.Update(transmissionType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var transmission = await _context.TransmissionTypes.FindAsync(id);

            if (transmission == null)
                return;

            transmission.IsActive = false;
            transmission.ModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
