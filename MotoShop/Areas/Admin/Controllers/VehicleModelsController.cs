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
    public class VehicleModelsController : Controller
    {
        private readonly IVehicleModelService _modelService;
        private readonly IVehicleBrandService _brandService;

        public VehicleModelsController(
            IVehicleModelService modelService,
            IVehicleBrandService brandService)
        {
            _modelService = modelService;
            _brandService = brandService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _modelService.GetAllAsync());
        }

        public async Task<IActionResult> Create()
        {
            var vm = new VehicleModelViewModel();

            await LoadBrands(vm);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleModelViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await LoadBrands(vm);
                return View(vm);
            }

            var model = new VehicleModel
            {
                Name = vm.Name,
                Description = vm.Description,
                VehicleBrandId = vm.VehicleBrandId,
                CreatedOn = DateTime.UtcNow,
                IsActive = true
            };

            await _modelService.AddAsync(model);

            TempData["Success"] = "Vehicle Model created successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await _modelService.GetByIdAsync(id);

            if (model == null)
                return NotFound();

            var vm = new VehicleModelViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                VehicleBrandId = model.VehicleBrandId
            };

            await LoadBrands(vm);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VehicleModelViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await LoadBrands(vm);
                return View(vm);
            }

            var model = await _modelService.GetByIdAsync(vm.Id);

            if (model == null)
                return NotFound();

            model.Name = vm.Name;
            model.Description = vm.Description;
            model.VehicleBrandId = vm.VehicleBrandId;
            model.ModifiedOn = DateTime.UtcNow;

            await _modelService.UpdateAsync(model);

            TempData["Success"] = "Vehicle Model updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _modelService.DeleteAsync(id);

            TempData["Success"] = "Vehicle Model deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadBrands(VehicleModelViewModel vm)
        {
            var brands = await _brandService.GetAllAsync();

            vm.Brands = brands.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = $"{x.VehicleCategory?.Name} - {x.Name}"
            }).ToList();
        }
    }
}
