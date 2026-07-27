using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MotoShop.Constants;
using MotoShop.Data;
using MotoShop.Interfaces.Services;

namespace MotoShop.Areas.Buyer.Controllers
{
    [Area("Buyer")]
    [Authorize(Roles = Roles.Buyer)]
    public class DashboardController : Controller
    {
        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBidService _bidService;

        public DashboardController(
            UserManager<ApplicationUser> userManager,
            IBidService bidService)
        {
            _userManager = userManager;
            _bidService = bidService;
        }

        public async Task<IActionResult> Index()
        {
            var buyer = await _userManager.GetUserAsync(User);

            if (buyer == null)
                return Challenge();

            var bids = await _bidService.GetBuyerBidsAsync(buyer.Id);

            ViewBag.WonAuctions = bids.Count(x => x.IsWinningBid);

            ViewBag.ActiveBids = bids.Count(x => !x.Auction.IsClosed);

            ViewBag.TotalBids = bids.Count;

            ViewBag.PurchasedVehicles = 0;

            return View();
        }
    }
}
