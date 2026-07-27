using MotoShop.Data;
using MotoShop.Interfaces.Repositories;
using MotoShop.Models;
using Microsoft.EntityFrameworkCore;

namespace MotoShop.Repositories
{
    public class VehicleColorRepository: IVehicleColorRepository
    {
        private readonly ApplicationDbContext _context;

        public VehicleColorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<VehicleColor>> GetAllAsync()
        {
            return await _context.VehicleColors
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<VehicleColor?> GetByIdAsync(int id)
        {
            return await _context.VehicleColors
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(VehicleColor vehicleColor)
        {
            await _context.VehicleColors.AddAsync(vehicleColor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(VehicleColor vehicleColor)
        {
            _context.VehicleColors.Update(vehicleColor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var vehicleColor = await _context.VehicleColors.FindAsync(id);

            if (vehicleColor == null)
                return;

            vehicleColor.IsActive = false;
            vehicleColor.ModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
