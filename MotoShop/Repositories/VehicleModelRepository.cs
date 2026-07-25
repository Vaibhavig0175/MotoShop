using MotoShop.Data;
using MotoShop.Interfaces.Repositories;
using MotoShop.Models;
using Microsoft.EntityFrameworkCore;

namespace MotoShop.Repositories
{
    public class VehicleModelRepository: IVehicleModelRepository
    {
        private readonly ApplicationDbContext _context;

        public VehicleModelRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<VehicleModel>> GetAllAsync()
        {
            return await _context.VehicleModels
                .Include(x => x.VehicleBrand)
                .ThenInclude(x => x.VehicleCategory)
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<VehicleModel?> GetByIdAsync(int id)
        {
            return await _context.VehicleModels
                .Include(x => x.VehicleBrand)
                .ThenInclude(x => x.VehicleCategory)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(VehicleModel model)
        {
            await _context.VehicleModels.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(VehicleModel model)
        {
            _context.VehicleModels.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var model = await _context.VehicleModels.FindAsync(id);

            if (model == null)
                return;

            model.IsActive = false;
            model.ModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
