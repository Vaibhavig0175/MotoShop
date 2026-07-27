using Microsoft.EntityFrameworkCore;
using MotoShop.Data;
using MotoShop.Enums;
using MotoShop.Interfaces.Repositories;
using MotoShop.Interfaces.Services;
using MotoShop.Models;

namespace MotoShop.Repositories
{
    public class VehicleRepository: IVehicleRepository
    {
        private readonly ApplicationDbContext _context;

        public INotificationService _notificationService { get; }

        public VehicleRepository(ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<List<Vehicle>> GetAllAsync()
        {
            return await _context.Vehicles
                .Include(x => x.VehicleCategory)
                .Include(x => x.VehicleBrand)
                .Include(x => x.VehicleModel)
                .Include(x => x.FuelType)
                .Include(x => x.Seller)
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
        }

        public async Task<List<Vehicle>> GetSellerVehiclesAsync(string sellerId)
        {
            return await _context.Vehicles
                .Include(x => x.VehicleCategory)
                .Include(x => x.VehicleBrand)
                .Include(x => x.VehicleModel)
                .Include(x => x.FuelType)
                .Where(x => x.SellerId == sellerId && x.IsActive)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
        }

        public async Task<List<Vehicle>> GetPendingVehiclesAsync()
        {
            return await _context.Vehicles
                 .Include(x => x.VehicleCategory)
                 .Include(x => x.VehicleBrand)
                 .Include(x => x.VehicleModel)
                 .Include(x => x.FuelType)
                 .Include(x => x.Images)
                 .Include(x => x.Seller)
                 .Where(x => x.Status == VehicleStatus.PendingApproval && x.IsActive)
                 .OrderByDescending(x => x.CreatedOn)
                 .ToListAsync();
        }

        public async Task<List<Vehicle>> GetApprovedVehiclesAsync()
        {
            return await _context.Vehicles
                .Include(x => x.VehicleCategory)
                .Include(x => x.VehicleBrand)
                .Include(x => x.VehicleModel)
                .Include(x => x.Seller)
                .Where(x => x.Status == MotoShop.Enums.VehicleStatus.Approved)
                .ToListAsync();
        }

        public async Task<Vehicle?> GetByIdAsync(int id)
        {
            return await _context.Vehicles
                .Include(x => x.VehicleCategory)
                .Include(x => x.VehicleBrand)
                .Include(x => x.VehicleModel)
                .Include(x => x.FuelType)
                .Include(x => x.Seller)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(Vehicle vehicle)
        {
            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);

            if (vehicle == null)
                return;

            vehicle.IsActive = false;
            vehicle.ModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
        public async Task AddImageAsync(VehicleImage image)
        {
            await _context.VehicleImages.AddAsync(image);
            await _context.SaveChangesAsync();
        }

        public async Task<List<VehicleImage>> GetImagesAsync(int vehicleId)
        {
            return await _context.VehicleImages
                .Where(x => x.VehicleId == vehicleId)
                .OrderByDescending(x => x.IsPrimary)
                .ToListAsync();
        }
        public async Task<Vehicle?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Vehicles
                .Include(x => x.VehicleCategory)
                .Include(x => x.VehicleBrand)
                .Include(x => x.VehicleModel)
                .Include(x => x.FuelType)
                .Include(x => x.Images)
                .Include(x => x.Seller)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task ApproveAsync(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);

            if (vehicle == null)
                return;

            vehicle.Status = VehicleStatus.Approved;
            vehicle.ModifiedOn = DateTime.UtcNow;

            _context.Update(vehicle);
            await _notificationService.CreateNotificationAsync(
                                        vehicle.SellerId,
                                        "Vehicle Approved",
                                        "Congratulations! Your vehicle has been approved by the administrator.",
                                        NotificationType.Success,
                                        "/Seller/Vehicles");

            await _context.SaveChangesAsync();
        }
        public async Task RejectAsync(int id, string reason)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);

            if (vehicle == null)
                return;

            vehicle.Status = VehicleStatus.Rejected;
            vehicle.RejectionReason = reason;
            vehicle.ModifiedOn = DateTime.UtcNow;

            _context.Vehicles.Update(vehicle);

            await _notificationService.CreateNotificationAsync(
                                        vehicle.SellerId,
                                        "Vehicle Rejected",
                                        $"Reason: {reason}",
                                        NotificationType.Error,
                                        "/Seller/Vehicles");

            await _context.SaveChangesAsync();
        }
    }
}
