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
                .Where(v => v.SellerId == seller.Id)
                .ToListAsync();

            ViewBag.TotalVehicles = vehicles.Count;

            ViewBag.PendingVehicles = vehicles.Count(v =>
                v.Status == VehicleStatus.Pending);

            ViewBag.ApprovedVehicles = vehicles.Count(v =>
                v.Status == VehicleStatus.Approved);

            ViewBag.RejectedVehicles = vehicles.Count(v =>
                v.Status == VehicleStatus.Rejected);

            ViewBag.ActiveAuctions = await _context.Auctions
                .Where(a =>
                    a.Vehicle.SellerId == seller.Id &&
                    a.Status == AuctionStatus.Live)
                .CountAsync();

            ViewBag.SoldVehicles = await _context.Auctions
                .Where(a =>
                    a.Vehicle.SellerId == seller.Id &&
                    a.Status == AuctionStatus.Closed &&
                    a.WinnerId != null)
                .CountAsync();

            ViewBag.TotalEarnings = await _context.Auctions
                .Where(a =>
                    a.Vehicle.SellerId == seller.Id &&
                    a.Status == AuctionStatus.Closed &&
                    a.WinnerId != null)
                .SumAsync(a => (decimal?)a.CurrentBid) ?? 0;

            return View();
        }
    }
}
