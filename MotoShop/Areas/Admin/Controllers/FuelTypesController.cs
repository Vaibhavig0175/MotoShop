using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoShop.Interfaces.Services;
using MotoShop.Models;

namespace MotoShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class IFuelTypeController : Controller
    {
        private readonly IFuelTypeService _fuelTypeService;

        public IFuelTypeController(IFuelTypeService fuelTypeService)
        {
            _fuelTypeService = fuelTypeService;
        }

        public async Task<IActionResult> Index()
        {
            var fuelTypes = await _fuelTypeService.GetAllAsync();
            return View(fuelTypes);
        }

        public IActionResult Create()
        {
            return View(new FuelType());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FuelType model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.IsActive = true;
            model.CreatedOn = DateTime.UtcNow;

            await _fuelTypeService.AddAsync(model);

            TempData["Success"] = "Fuel Type added successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var fuelType = await _fuelTypeService.GetByIdAsync(id);

            if (fuelType == null)
                return NotFound();

            return View(fuelType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FuelType model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var fuelType = await _fuelTypeService.GetByIdAsync(model.Id);

            if (fuelType == null)
                return NotFound();

            fuelType.Name = model.Name;
            fuelType.Description = model.Description;
            fuelType.ModifiedOn = DateTime.UtcNow;

            await _fuelTypeService.UpdateAsync(fuelType);

            TempData["Success"] = "Fuel Type updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _fuelTypeService.DeleteAsync(id);

            TempData["Success"] = "Fuel Type deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
