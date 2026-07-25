using MotoShop.Data;
using MotoShop.Interfaces.Repositories;
using MotoShop.Models;
using Microsoft.EntityFrameworkCore;

namespace MotoShop.Repositories
{
    public class VehicleBrandRepository: IVehicleBrandRepository
    {
        private readonly ApplicationDbContext _context;

        public VehicleBrandRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<VehicleBrand>> GetAllAsync()
        {
            return await _context.VehicleBrands
                .Include(x => x.VehicleCategory)
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<VehicleBrand?> GetByIdAsync(int id)
        {
            return await _context.VehicleBrands.FindAsync(id);
        }

        public async Task AddAsync(VehicleBrand brand)
        {
            await _context.VehicleBrands.AddAsync(brand);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(VehicleBrand brand)
        {
            _context.VehicleBrands.Update(brand);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var brand = await _context.VehicleBrands.FindAsync(id);

            if (brand == null)
                return;

            brand.IsActive = false;
            brand.ModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
        public async Task<List<VehicleBrand>> GetByCategoryAsync(int categoryId)
        {
            return await _context.VehicleBrands
                .Where(x => x.IsActive && x.VehicleCategoryId == categoryId)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
    }
}
