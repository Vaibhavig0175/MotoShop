using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MotoShop.Constants;
using MotoShop.Data;
using MotoShop.Enums;
using Microsoft.EntityFrameworkCore;

namespace MotoShop.Areas.Seller.Controllers
{
    [Area("Seller")]
    [Authorize(Roles = "Seller")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var seller = await _userManager.GetUserAsync(User);

            if (seller == null)
                return Challenge();

            var vehicles = await _context.Vehicles
                .Where(x => x.SellerId == seller.Id)
                .ToListAsync();

            ViewBag.TotalVehicles = vehicles.Count;

            ViewBag.PendingVehicles = vehicles.Count(x =>
                x.Status == VehicleStatus.Pending);

            ViewBag.ApprovedVehicles = vehicles.Count(x =>
                x.Status == VehicleStatus.Approved);

            ViewBag.RejectedVehicles = vehicles.Count(x =>
                x.Status == VehicleStatus.Rejected);

            ViewBag.ActiveAuctions = await _context.Auctions
                .CountAsync(x =>
                    x.Vehicle.SellerId == seller.Id &&
                    !x.IsClosed);

            ViewBag.SoldVehicles = await _context.Auctions
                .CountAsync(x =>
                    x.Vehicle.SellerId == seller.Id &&
                    x.IsClosed &&
                    x.WinnerId != null);

            ViewBag.TotalEarnings = await _context.Auctions
                .Where(x =>
                    x.Vehicle.SellerId == seller.Id &&
                    x.IsClosed &&
                    x.WinnerId != null)
                .SumAsync(x => (decimal?)x.CurrentBid) ?? 0;

            return View();
        }
    }
}
