using MotoShop.Enums;
using MotoShop.Interfaces.Repositories;
using MotoShop.Interfaces.Services;
using MotoShop.Models;

namespace MotoShop.Services
{
    public class VehicleService: IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IWebHostEnvironment _environment;

        public VehicleService(
            IVehicleRepository vehicleRepository,
            IWebHostEnvironment environment)
        {
            _vehicleRepository = vehicleRepository;
            _environment = environment;
        }

        public async Task<List<Vehicle>> GetAllAsync()
        {
            return await _vehicleRepository.GetAllAsync();
        }

        public async Task<List<Vehicle>> GetSellerVehiclesAsync(string sellerId)
        {
            return await _vehicleRepository.GetSellerVehiclesAsync(sellerId);
        }

        public async Task<List<Vehicle>> GetPendingVehiclesAsync()
        {
            return await _vehicleRepository.GetPendingVehiclesAsync();
        }

        public async Task<List<Vehicle>> GetApprovedVehiclesAsync()
        {
            return await _vehicleRepository.GetApprovedVehiclesAsync();
        }

        public async Task<Vehicle?> GetByIdAsync(int id)
        {
            return await _vehicleRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Vehicle vehicle)
        {
            vehicle.Status = VehicleStatus.PendingApproval;
            vehicle.CreatedOn = DateTime.UtcNow;
            vehicle.ModifiedOn = DateTime.UtcNow;
            vehicle.IsActive = true;

            await _vehicleRepository.AddAsync(vehicle);
        }

        public async Task UpdateAsync(Vehicle vehicle)
        {
            vehicle.ModifiedOn = DateTime.UtcNow;

            await _vehicleRepository.UpdateAsync(vehicle);
        }

        public async Task DeleteAsync(int id)
        {
            await _vehicleRepository.DeleteAsync(id);
        }

        public async Task ApproveVehicleAsync(int id)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);

            if (vehicle == null)
                return;

            vehicle.Status = VehicleStatus.Approved;
            vehicle.RejectionReason = null;
            vehicle.ModifiedOn = DateTime.UtcNow;

            await _vehicleRepository.UpdateAsync(vehicle);
        }

        public async Task RejectVehicleAsync(int id, string reason)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);

            if (vehicle == null)
                return;

            vehicle.Status = VehicleStatus.Rejected;
            vehicle.RejectionReason = reason;
            vehicle.ModifiedOn = DateTime.UtcNow;

            await _vehicleRepository.UpdateAsync(vehicle);
        }
        public async Task UploadImagesAsync(int vehicleId, List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return;

            var folder = Path.Combine(_environment.WebRootPath, "uploads", "vehicles");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var isFirst = true;

            foreach (var file in files)
            {
                if (file.Length <= 0)
                    continue;

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                var path = Path.Combine(folder, fileName);

                using var stream = new FileStream(path, FileMode.Create);

                await file.CopyToAsync(stream);

                await _vehicleRepository.AddImageAsync(new VehicleImage
                {
                    VehicleId = vehicleId,
                    ImageUrl = "/uploads/vehicles/" + fileName,
                    IsPrimary = isFirst
                });

                isFirst = false;
            }
        }

        public async Task<List<VehicleImage>> GetImagesAsync(int vehicleId)
        {
            return await _vehicleRepository.GetImagesAsync(vehicleId);
        }
        public async Task<Vehicle?> GetByIdWithDetailsAsync(int id)
        {
            return await _vehicleRepository.GetByIdWithDetailsAsync(id);
        }
        public async Task ApproveAsync(int id)
        {
            await _vehicleRepository.ApproveAsync(id);
        }
        public async Task RejectAsync(int id, string reason)
        {
            await _vehicleRepository.RejectAsync(id, reason);
        }
    }
}
