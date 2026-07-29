using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MotoShop.Data;
using MotoShop.Enums;
using MotoShop.Interfaces.Repositories;
using MotoShop.Interfaces.Services;
using MotoShop.Models;
using MotoShop.Services;
using MotoShop.ViewModels.Seller;

namespace MotoShop.Areas.Seller.Controllers
{
    [Area("Seller")]
    [Authorize(Roles = "Seller")]
    public class VehiclesController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly IVehicleCategoryRepository _categoryRepository;
        private readonly IVehicleBrandService _brandService;
        private readonly IVehicleModelService _modelService;
        private readonly IFuelTypeService _fuelTypeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;
        private readonly INotificationService _notificationService;
        private readonly ApplicationDbContext _context;


        public VehiclesController(
            IVehicleService vehicleService,
            IVehicleCategoryRepository categoryRepository,
            IVehicleBrandService brandService,
            IVehicleModelService modelService,
            IFuelTypeService fuelTypeService,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment environment,
            INotificationService notificationService,
            ApplicationDbContext context)
        {
            _vehicleService = vehicleService;
            _categoryRepository = categoryRepository; 
            _brandService = brandService;
            _modelService = modelService;
            _fuelTypeService = fuelTypeService;
            _userManager = userManager;
            _environment = environment;
            _notificationService = notificationService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var seller = await _userManager.GetUserAsync(User);

            var vehicles = await _vehicleService.GetSellerVehiclesAsync(seller!.Id);

            return View(vehicles);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new AddVehicleViewModel();

            await LoadDropdowns(vm);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddVehicleViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns(vm);
                return View(vm);
            }

            var seller = await _userManager.GetUserAsync(User);

            var vehicle = new Vehicle
            {
                SellerId = seller!.Id,
                Title = vm.Title,
                Description = vm.Description,
                VehicleCategoryId = vm.VehicleCategoryId,
                VehicleBrandId = vm.VehicleBrandId,
                VehicleModelId = vm.VehicleModelId,
                FuelTypeId = vm.FuelTypeId,
                Transmission = vm.Transmission,
                Color = vm.Color,
                RegistrationNumber = vm.RegistrationNumber,
                ManufacturingYear = vm.ManufacturingYear,
                KilometersDriven = vm.KilometersDriven,
                Owners = vm.Owners,
                StartingPrice = vm.StartingPrice,
                ReservePrice = vm.ReservePrice
            };

            await _vehicleService.AddAsync(vehicle);

            var admin = await _userManager.GetUsersInRoleAsync("Admin");

            foreach (var user in admin)
            {
                await _notificationService.CreateNotificationAsync(
                    user.Id,
                    "New Vehicle Submitted",
                    "A seller has submitted a vehicle for approval.",
                    NotificationType.Info,
                    "/Admin/VehicleApproval");
            }

            if (vm.Images != null && vm.Images.Any())
            {
                await _vehicleService.UploadImagesAsync(vehicle.Id, vm.Images);
            }

            TempData["Success"] = "Vehicle submitted successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);

            if (vehicle == null)
                return NotFound();

            return View(vehicle);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);

            if (vehicle == null)
                return NotFound();

            var vm = new AddVehicleViewModel
            {
                Title = vehicle.Title,
                Description = vehicle.Description,
                VehicleCategoryId = vehicle.VehicleCategoryId,
                VehicleBrandId = vehicle.VehicleBrandId,
                VehicleModelId = vehicle.VehicleModelId,
                FuelTypeId = vehicle.FuelTypeId,
                Transmission = vehicle.Transmission,
                Color = vehicle.Color,
                RegistrationNumber = vehicle.RegistrationNumber,
                ManufacturingYear = vehicle.ManufacturingYear,
                KilometersDriven = vehicle.KilometersDriven,
                Owners = vehicle.Owners,
                StartingPrice = vehicle.StartingPrice,
                ReservePrice = vehicle.ReservePrice
            };

            await LoadDropdowns(vm);

            ViewBag.VehicleId = id;

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AddVehicleViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns(vm);
                return View(vm);
            }

            var vehicle = await _vehicleService.GetByIdAsync(id);

            if (vehicle == null)
                return NotFound();

            vehicle.Title = vm.Title;
            vehicle.Description = vm.Description;
            vehicle.VehicleCategoryId = vm.VehicleCategoryId;
            vehicle.VehicleBrandId = vm.VehicleBrandId;
            vehicle.VehicleModelId = vm.VehicleModelId;
            vehicle.FuelTypeId = vm.FuelTypeId;
            vehicle.Transmission = vm.Transmission;
            vehicle.Color = vm.Color;
            vehicle.RegistrationNumber = vm.RegistrationNumber;
            vehicle.ManufacturingYear = vm.ManufacturingYear;
            vehicle.KilometersDriven = vm.KilometersDriven;
            vehicle.Owners = vm.Owners;
            vehicle.StartingPrice = vm.StartingPrice;
            vehicle.ReservePrice = vm.ReservePrice;
            vehicle.Status = VehicleStatus.PendingApproval;
            vehicle.RejectionReason = null;
            vehicle.ModifiedOn = DateTime.UtcNow;

            await _vehicleService.UpdateAsync(vehicle);

            TempData["Success"] = "Vehicle updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _vehicleService.DeleteAsync(id);

            TempData["Success"] = "Vehicle deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdowns(AddVehicleViewModel vm)
        {
            vm.Categories = (await _categoryRepository.GetAllAsync())
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });

            vm.Brands = (await _brandService.GetAllAsync())
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });

            vm.Models = (await _modelService.GetAllAsync())
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });

            vm.FuelTypes = (await _fuelTypeService.GetAllAsync())
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
        }
        public async Task<IActionResult> SoldVehicles()
        {
            var sellerId = _userManager.GetUserId(User);

            var auctions = await _context.Auctions
                .Include(a => a.Vehicle)
                    .ThenInclude(v => v.VehicleBrand)
                .Include(a => a.Vehicle)
                    .ThenInclude(v => v.VehicleModel)
                .Include(a => a.Vehicle)
                    .ThenInclude(v => v.Images)
                .Include(a => a.Winner)
                .Where(a =>
                    a.Vehicle.SellerId == sellerId &&
                    a.Status == AuctionStatus.Closed &&
                    a.WinnerId != null)
                .OrderByDescending(a => a.ClosedOn)
                .ToListAsync();

            return View(auctions);
        }
    }
}
