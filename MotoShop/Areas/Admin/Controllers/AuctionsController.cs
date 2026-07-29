using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MotoShop.Areas.Admin.Models;
using MotoShop.Data;
using MotoShop.Enums;
using MotoShop.Interfaces.Services;
using MotoShop.Models;

namespace MotoShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AuctionsController : Controller
    {
        private readonly IAuctionService _auctionService;
        private readonly ApplicationDbContext _context;

        public AuctionsController(IAuctionService auctionService, ApplicationDbContext context)
        {
            _auctionService = auctionService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var auctions = await _context.Auctions
                .Include(a => a.Vehicle)
                    .ThenInclude(v => v.VehicleBrand)
                .Include(a => a.Vehicle)
                    .ThenInclude(v => v.VehicleModel)
                .Include(a => a.Vehicle)
                    .ThenInclude(v => v.Seller)
                .ToListAsync();

            var vm = new AuctionDashboardViewModel
            {
                TotalAuctions = auctions.Count,

                UpcomingAuctions = auctions.Count(x => x.Status == AuctionStatus.Upcoming),

                LiveAuctions = auctions.Count(x => x.Status == AuctionStatus.Live),

                ClosedAuctions = auctions.Count(x => x.Status == AuctionStatus.Closed),

                CancelledAuctions = auctions.Count(x => x.Status == AuctionStatus.Cancelled),

                Auctions = auctions.Select(a => new AuctionListViewModel
                {
                    Id = a.Id,

                    VehicleName = $"{a.Vehicle.VehicleBrand.Name} {a.Vehicle.VehicleModel.Name}",

                    SellerName = a.Vehicle.Seller.FullName,

                    StartingPrice = a.StartingPrice,

                    CurrentBid = a.CurrentBid,

                    StartTime = a.StartTime,

                    EndTime = a.EndTime,

                    Status = a.Status.ToString()

                }).ToList()
            };


            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var auction = await _context.Auctions

                .Include(a => a.Vehicle)
                    .ThenInclude(v => v.Images)

                .Include(a => a.Vehicle)
                    .ThenInclude(v => v.VehicleBrand)

                .Include(a => a.Vehicle)
                    .ThenInclude(v => v.VehicleModel)

                .Include(a => a.Vehicle)
                    .ThenInclude(v => v.VehicleCategory)

                .Include(a => a.Vehicle)
                    .ThenInclude(v => v.FuelType)

                .Include(a => a.Vehicle)
                    .ThenInclude(v => v.Seller)

                .Include(a => a.Bids)
                    .ThenInclude(b => b.Buyer)

                    .Include(a => a.Winner)

                .FirstOrDefaultAsync(a => a.Id == id);

            if (auction == null)
                return NotFound();

            return View(auction);
        }

        public async Task<IActionResult> Create()
        {
            var vehicles = await _context.Vehicles
                .Include(x => x.VehicleBrand)
                .Include(x => x.VehicleModel)
                .Where(x => x.Status == VehicleStatus.Approved)
                .ToListAsync();

            var vm = new CreateAuctionViewModel
            {
                StartTime = DateTime.Now.AddMinutes(10),
                EndTime = DateTime.Now.AddDays(1),

                Vehicles = vehicles.Select(v => new SelectListItem
                {
                    Value = v.Id.ToString(),
                    Text = $"{v.VehicleBrand.Name} {v.VehicleModel.Name} ({v.RegistrationNumber})"
                })
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAuctionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var vehicles = await _context.Vehicles
                    .Include(x => x.VehicleBrand)
                    .Include(x => x.VehicleModel)
                    .ToListAsync();

                model.Vehicles = vehicles.Select(v => new SelectListItem
                {
                    Value = v.Id.ToString(),
                    Text = $"{v.VehicleBrand.Name} {v.VehicleModel.Name}"
                });

                return View(model);
            }

            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(x => x.Id == model.VehicleId);

            if (vehicle == null)
                return NotFound();

            var auction = new Auction
            {
                VehicleId = model.VehicleId,

                StartingPrice = model.StartingPrice,

                CurrentBid = model.StartingPrice,

                ReservePrice = model.ReservePrice,

                StartTime = model.StartTime,

                EndTime = model.EndTime,

                Status = AuctionStatus.Upcoming,

                CreatedOn = DateTime.Now,

                IsActive = true
            };

            //var seller = auction.Vehicle.Seller;

            _context.Auctions.Add(auction);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Auction created successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var auction = await _context.Auctions
                .Include(a => a.Vehicle)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (auction == null)
                return NotFound();

            if (auction.Status == AuctionStatus.Live ||
                auction.Status == AuctionStatus.Closed)
            {
                TempData["Error"] = "Live or Closed auctions cannot be edited.";

                return RedirectToAction(nameof(Index));
            }

            var vehicles = await _context.Vehicles
                .Include(v => v.VehicleBrand)
                .Include(v => v.VehicleModel)
                .Where(v => v.Status == VehicleStatus.Approved)
                .ToListAsync();

            var vm = new CreateAuctionViewModel
            {
                Id = auction.Id,

                VehicleId = auction.VehicleId,

                StartingPrice = auction.StartingPrice,

                ReservePrice = auction.ReservePrice,

                StartTime = auction.StartTime,

                EndTime = auction.EndTime,

                Vehicles = vehicles.Select(v => new SelectListItem
                {
                    Value = v.Id.ToString(),

                    Text = $"{v.VehicleBrand.Name} {v.VehicleModel.Name} ({v.RegistrationNumber})"
                })
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateAuctionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var vehicles = await _context.Vehicles
                    .Include(v => v.VehicleBrand)
                    .Include(v => v.VehicleModel)
                    .ToListAsync();

                model.Vehicles = vehicles.Select(v => new SelectListItem
                {
                    Value = v.Id.ToString(),

                    Text = $"{v.VehicleBrand.Name} {v.VehicleModel.Name}"
                });

                return View(model);
            }

            var auction = await _context.Auctions
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (auction == null)
                return NotFound();

            if (auction.Status == AuctionStatus.Live ||
                auction.Status == AuctionStatus.Closed)
            {
                TempData["Error"] = "Live or Closed auctions cannot be edited.";

                return RedirectToAction(nameof(Index));
            }

            auction.VehicleId = model.VehicleId;

            auction.StartingPrice = model.StartingPrice;

            auction.ReservePrice = model.ReservePrice;

            auction.StartTime = model.StartTime;

            auction.EndTime = model.EndTime;

            _context.Update(auction);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Auction updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var auction = await _context.Auctions
                .Include(a => a.Vehicle)
                    .ThenInclude(v => v.VehicleBrand)
                .Include(a => a.Vehicle)
                    .ThenInclude(v => v.VehicleModel)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (auction == null)
                return NotFound();

            return View(auction);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var auction = await _context.Auctions
                .FirstOrDefaultAsync(x => x.Id == id);

            if (auction == null)
                return NotFound();

            if (auction.Status == AuctionStatus.Live)
            {
                TempData["Error"] = "Live auction cannot be deleted.";

                return RedirectToAction(nameof(Index));
            }

            _context.Auctions.Remove(auction);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Auction deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Start(int id)
        {
            var auction = await _context.Auctions
                .FirstOrDefaultAsync(a => a.Id == id);

            if (auction == null)
                return NotFound();

            if (auction.Status != AuctionStatus.Upcoming)
            {
                TempData["Error"] = "Only upcoming auctions can be started.";

                return RedirectToAction(nameof(Index));
            }

            auction.Status = AuctionStatus.Live;

            auction.StartTime = DateTime.Now;

            _context.Update(auction);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Auction started successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Close(int id)
        {
            var auction = await _context.Auctions
                .Include(a => a.Bids)
                    .ThenInclude(b => b.Buyer)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (auction == null)
                return NotFound();

            if (auction.Status != AuctionStatus.Live)
            {
                TempData["Error"] = "Only live auctions can be closed.";

                return RedirectToAction(nameof(Index));
            }

            auction.Status = AuctionStatus.Closed;

            auction.EndTime = DateTime.Now;

            var winningBid = auction.Bids
                            .OrderByDescending(x => x.Amount)
                            .FirstOrDefault();

            if (winningBid != null)
            {
                auction.CurrentBid = winningBid.Amount;

                auction.WinnerId = winningBid.BuyerId;

                auction.WinningBidAmount = winningBid.Amount;

                auction.ClosedOn = DateTime.Now;

                auction.Status = AuctionStatus.Closed;

                auction.EndTime = DateTime.Now;

                // Optional: Mark vehicle sold
                // auction.Vehicle.Status = VehicleStatus.Sold;
            }
            else
            {
                auction.Status = AuctionStatus.Closed;

                auction.ClosedOn = DateTime.Now;
            }

            _context.Notifications.Add(new Notification
            {
                UserId = winningBid.BuyerId,

                Title = "Congratulations!",

                Message = $"You won the auction for {auction.Vehicle.Title}.",

                IsRead = false,

                CreatedOn = DateTime.Now
            });
            _context.Notifications.Add(new Notification
            {
                UserId = auction.Vehicle.SellerId,

                Title = "Auction Closed",

                Message = $"Your vehicle '{auction.Vehicle.Title}' has been sold.",

                IsRead = false,

                CreatedOn = DateTime.Now
            });
            _context.Payments.Add(new Payment
            {
                AuctionId = auction.Id,

                BuyerId = winningBid.BuyerId,

                Amount = winningBid.Amount,

                Status = PaymentStatus.Pending,

                CreatedOn = DateTime.Now
            });
            _context.Update(auction);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Auction closed successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
