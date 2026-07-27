using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoShop.Interfaces.Services;
using MotoShop.Models;

namespace MotoShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class VehicleColorsController : Controller
    {
        private readonly IVehicleColorService _vehicleColorService;

        public VehicleColorsController(IVehicleColorService vehicleColorService)
        {
            _vehicleColorService = vehicleColorService;
        }

        public async Task<IActionResult> Index()
        {
            var colors = await _vehicleColorService.GetAllAsync();
            return View(colors);
        }

        public IActionResult Create()
        {
            return View(new VehicleColor());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleColor model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.IsActive = true;
            model.CreatedOn = DateTime.UtcNow;

            await _vehicleColorService.AddAsync(model);

            TempData["Success"] = "Vehicle Color added successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var color = await _vehicleColorService.GetByIdAsync(id);

            if (color == null)
                return NotFound();

            return View(color);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VehicleColor model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var color = await _vehicleColorService.GetByIdAsync(model.Id);

            if (color == null)
                return NotFound();

            color.Name = model.Name;
            color.Description = model.Description;
            color.ModifiedOn = DateTime.UtcNow;

            await _vehicleColorService.UpdateAsync(color);

            TempData["Success"] = "Vehicle Color updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _vehicleColorService.DeleteAsync(id);

            TempData["Success"] = "Vehicle Color deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
