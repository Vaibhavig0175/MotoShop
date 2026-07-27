using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MotoShop.Areas.Admin.Models;
using MotoShop.Interfaces.Services;
using MotoShop.Models;

namespace MotoShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class VehicleBrandsController : Controller
    {
        private readonly IVehicleBrandService _brandService;
        private readonly IVehicleCategoryService _categoryService;

        public VehicleBrandsController(
            IVehicleBrandService brandService,
            IVehicleCategoryService categoryService)
        {
            _brandService = brandService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _brandService.GetAllAsync());
        }

        public async Task<IActionResult> Create()
        {
            var model = new VehicleBrandViewModel();

            await LoadCategories(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleBrandViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategories(model);
                return View(model);
            }

            var brand = new VehicleBrand
            {
                Name = model.Name,
                Description = model.Description,
                VehicleCategoryId = model.VehicleCategoryId,
                IsActive = true,
                CreatedOn = DateTime.UtcNow
            };

            await _brandService.AddAsync(brand);

            TempData["Success"] = "Brand created successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var brand = await _brandService.GetByIdAsync(id);

            if (brand == null)
                return NotFound();

            var model = new VehicleBrandViewModel
            {
                Id = brand.Id,
                Name = brand.Name,
                Description = brand.Description,
                VehicleCategoryId = brand.VehicleCategoryId
            };

            await LoadCategories(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VehicleBrandViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategories(model);
                return View(model);
            }

            var brand = await _brandService.GetByIdAsync(model.Id);

            if (brand == null)
                return NotFound();

            brand.Name = model.Name;
            brand.Description = model.Description;
            brand.VehicleCategoryId = model.VehicleCategoryId;
            brand.ModifiedOn = DateTime.UtcNow;

            await _brandService.UpdateAsync(brand);

            TempData["Success"] = "Brand updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _brandService.DeleteAsync(id);

            TempData["Success"] = "Brand deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadCategories(VehicleBrandViewModel model)
        {
            var categories = await _categoryService.GetAllAsync();

            model.Categories = categories
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToList();
        }

        [HttpGet]
        public async Task<JsonResult> GetBrandsByCategory(int categoryId)
        {
            var brands = await _brandService.GetByCategoryAsync(categoryId);

            return Json(brands.Select(x => new
            {
                id = x.Id,
                name = x.Name
            }));
        }
    }
}
