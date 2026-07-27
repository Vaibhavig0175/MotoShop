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
    public class AuctionsController : Controller
    {
        private readonly IAuctionService _auctionService;

        public AuctionsController(IAuctionService auctionService)
        {
            _auctionService = auctionService;
        }

        public async Task<IActionResult> Index()
        {
            var auctions = await _auctionService.GetAllAsync();

            return View(auctions);
        }

        public async Task<IActionResult> Details(int id)
        {
            var auction = await _auctionService.GetByIdAsync(id);

            if (auction == null)
                return NotFound();

            return View(auction);
        }

        public async Task<IActionResult> Create()
        {
            var vehicles = await _auctionService.GetApprovedVehiclesAsync();

            var vm = new AuctionVM
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),

                Vehicles = vehicles.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = $"{x.VehicleBrand?.Name} {x.VehicleModel?.Name}"
                })
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuctionVM vm)
        {
            if (!ModelState.IsValid)
            {
                var vehicles = await _auctionService.GetApprovedVehiclesAsync();

                vm.Vehicles = vehicles.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = $"{x.VehicleBrand?.Name} {x.VehicleModel?.Name}"
                });

                return View(vm);
            }

            if (await _auctionService.ExistsForVehicleAsync(vm.VehicleId))
            {
                ModelState.AddModelError("", "Auction already exists for this vehicle.");

                var vehicles = await _auctionService.GetApprovedVehiclesAsync();

                vm.Vehicles = vehicles.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = $"{x.VehicleBrand?.Name} {x.VehicleModel?.Name}"
                });

                return View(vm);
            }

            var auction = new Auction
            {
                VehicleId = vm.VehicleId,

                StartDate = vm.StartDate,

                EndDate = vm.EndDate,

                StartingPrice = vm.StartingPrice,

                ReservePrice = vm.ReservePrice,

                CurrentBid = vm.StartingPrice,

                IsClosed = false,

                CreatedOn = DateTime.UtcNow,

                IsActive = true
            };

            await _auctionService.AddAsync(auction);

            TempData["Success"] = "Auction Created Successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var auction = await _auctionService.GetByIdAsync(id);

            if (auction == null)
                return NotFound();

            var vehicles = await _auctionService.GetApprovedVehiclesAsync();

            var vm = new AuctionVM
            {
                Id = auction.Id,

                VehicleId = auction.VehicleId,

                StartDate = auction.StartDate,

                EndDate = auction.EndDate,

                StartingPrice = auction.StartingPrice,

                ReservePrice = auction.ReservePrice,

                Vehicles = vehicles.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),

                    Text = $"{x.VehicleBrand?.Name} {x.VehicleModel?.Name}"
                })
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AuctionVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var auction = await _auctionService.GetByIdAsync(vm.Id);

            if (auction == null)
                return NotFound();

            auction.VehicleId = vm.VehicleId;

            auction.StartDate = vm.StartDate;

            auction.EndDate = vm.EndDate;

            auction.StartingPrice = vm.StartingPrice;

            auction.ReservePrice = vm.ReservePrice;

            auction.ModifiedOn = DateTime.UtcNow;

            await _auctionService.UpdateAsync(auction);

            TempData["Success"] = "Auction Updated Successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var auction = await _auctionService.GetByIdAsync(id);

            if (auction == null)
                return NotFound();

            return View(auction);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _auctionService.DeleteAsync(id);

            TempData["Success"] = "Auction Deleted Successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
