using MotoShop.Data;
using MotoShop.Interfaces.Repositories;
using MotoShop.Models;
using Microsoft.EntityFrameworkCore;
namespace MotoShop.Repositories
{
    public class FuelTypeRepository: IFuelTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public FuelTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<FuelType>> GetAllAsync()
        {
            return await _context.FuelTypes
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<FuelType?> GetByIdAsync(int id)
        {
            return await _context.FuelTypes
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(FuelType fuelType)
        {
            await _context.FuelTypes.AddAsync(fuelType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FuelType fuelType)
        {
            _context.FuelTypes.Update(fuelType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var fuelType = await _context.FuelTypes.FindAsync(id);

            if (fuelType == null)
                return;

            fuelType.IsActive = false;
            fuelType.ModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
