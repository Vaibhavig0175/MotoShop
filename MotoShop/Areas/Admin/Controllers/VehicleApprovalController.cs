using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoShop.Interfaces.Services;

namespace MotoShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class VehicleApprovalController : Controller
    {
        private readonly IVehicleService _vehicleService;

        public VehicleApprovalController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public async Task<IActionResult> Index()
        {
            var vehicles = await _vehicleService.GetPendingVehiclesAsync();

            return View(vehicles);
        }

        public async Task<IActionResult> Details(int id)
        {
            var vehicle = await _vehicleService.GetByIdWithDetailsAsync(id);

            if (vehicle == null)
                return NotFound();

            return View(vehicle);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            await _vehicleService.ApproveAsync(id);

            TempData["Success"] = "Vehicle approved successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id, string reason)
        {
            await _vehicleService.RejectAsync(id, reason);

            TempData["Success"] = "Vehicle rejected successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
