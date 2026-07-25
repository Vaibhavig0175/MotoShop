using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoShop.Interfaces.Services;
using MotoShop.Models;

namespace MotoShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class VehicleCategoriesController : Controller
    {
        private readonly IVehicleCategoryService _service;

        public VehicleCategoriesController(IVehicleCategoryService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleCategory model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _service.AddAsync(model);

            TempData["Success"] = "Category created successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _service.GetByIdAsync(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VehicleCategory model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.ModifiedOn = DateTime.UtcNow;

            await _service.UpdateAsync(model);

            TempData["Success"] = "Category updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            TempData["Success"] = "Category deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
