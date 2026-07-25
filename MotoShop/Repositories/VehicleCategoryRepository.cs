using MotoShop.Data;
using MotoShop.Interfaces.Repositories;
using MotoShop.Models;
using Microsoft.EntityFrameworkCore;

namespace MotoShop.Repositories
{
    public class VehicleCategoryRepository : IVehicleCategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public VehicleCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<VehicleCategory>> GetAllAsync()
        {
            return await _context.VehicleCategories
         .Where(x => x.IsActive)
         .OrderBy(x => x.Name)
         .ToListAsync();
        }

        public async Task<VehicleCategory?> GetByIdAsync(int id)
        {
            return await _context.VehicleCategories.FindAsync(id);
        }

        public async Task AddAsync(VehicleCategory category)
        {
            _context.VehicleCategories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(VehicleCategory category)
        {
            _context.VehicleCategories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await GetByIdAsync(id);

            if (category == null)
                return;

            category.IsActive = false;
            category.ModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
