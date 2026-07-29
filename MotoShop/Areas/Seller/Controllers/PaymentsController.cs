using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoShop.Data;

namespace MotoShop.Areas.Seller.Controllers
{
    [Area("Seller")]
    [Authorize(Roles = "Seller")]
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public PaymentsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var sellerId = _userManager.GetUserId(User);

            var payments = await _context.Payments

                .Include(x => x.Buyer)

                .Include(x => x.Auction)

                .ThenInclude(x => x.Vehicle)

                .ThenInclude(x => x.VehicleBrand)

                .Include(x => x.Auction)

                .ThenInclude(x => x.Vehicle)

                .ThenInclude(x => x.VehicleModel)

                .Where(x => x.Auction.Vehicle.SellerId == sellerId)

                .OrderByDescending(x => x.CreatedOn)

                .ToListAsync();

            return View(payments);
        }
    }
}
