using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MotoShop.Data;
using MotoShop.Interfaces.Services;
using MotoShop.Models;
using Microsoft.AspNetCore.SignalR;
using MotoShop.Hubs;

namespace MotoShop.Areas.Buyer.Controllers
{
    [Area("Buyer")]
    [Authorize(Roles = "Buyer")]
    public class AuctionsController : Controller
    {
        private readonly IAuctionService _auctionService;
        private readonly IBidService _bidService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<AuctionHub> _hub;

        public AuctionsController(
            IAuctionService auctionService,
            IBidService bidService,
            UserManager<ApplicationUser> userManager,
            IHubContext<AuctionHub> hub)
        {
            _auctionService = auctionService;
            _bidService = bidService;
            _userManager = userManager;
            _hub = hub;
        }

        //---------------------------------------------------
        // LIVE AUCTIONS
        //---------------------------------------------------

        public async Task<IActionResult> Index()
        {
            var auctions = await _auctionService.GetActiveAsync();
            auctions = auctions
                    .Where(x => !x.IsClosed)
                    .ToList();

            return View(auctions);
        }

        //---------------------------------------------------
        // AUCTION DETAILS
        //---------------------------------------------------

        public async Task<IActionResult> Details(int id)
        {
            var auction = await _auctionService.GetByIdAsync(id);

            if (auction == null)
                return NotFound();

            ViewBag.Bids = await _bidService.GetAuctionBidsAsync(id);

            ViewBag.HighestBid =
                await _bidService.GetHighestBidAsync(id);

            return View(auction);
        }

        //---------------------------------------------------
        // PLACE BID
        //---------------------------------------------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceBid(int auctionId, decimal amount)
        {
            var auction = await _auctionService.GetByIdAsync(auctionId);

            if (auction == null)
                return NotFound();

            if (auction.IsClosed)
            {
                TempData["Error"] = "Auction is closed.";

                return RedirectToAction(nameof(Details), new { id = auctionId });
            }

            if (auction.EndDate <= DateTime.UtcNow)
            {
                TempData["Error"] = "Auction has already ended.";

                return RedirectToAction(nameof(Details), new { id = auctionId });
            }

            if (auction.StartDate > DateTime.UtcNow)
            {
                TempData["Error"] = "Auction has not started yet.";

                return RedirectToAction(nameof(Details), new { id = auctionId });
            }

            if (amount <= auction.CurrentBid)
            {
                TempData["Error"] =
                    $"Bid must be greater than ₹{auction.CurrentBid:N0}";

                return RedirectToAction(nameof(Details), new { id = auctionId });
            }

            var buyer = await _userManager.GetUserAsync(User);

            if (buyer == null)
                return Challenge();

            var bid = new Bid
            {
                AuctionId = auction.Id,

                BuyerId = buyer.Id,

                Amount = amount,

                BidTime = DateTime.UtcNow,

                CreatedOn = DateTime.UtcNow,

                IsActive = true
            };

            await _bidService.AddAsync(bid);

            await _hub.Clients
                .Group($"Auction-{auction.Id}")
                .SendAsync(
                    "ReceiveBid",
                    new
                    {
                        AuctionId = auction.Id,
                        Amount = bid.Amount,
                        Buyer = buyer.FullName,
                        Time = bid.CreatedOn.ToString("HH:mm:ss")
                    });

            auction.CurrentBid = amount;

            auction.WinnerId = buyer.Id;

            await _auctionService.UpdateAsync(auction);

            TempData["Success"] = "Bid placed successfully.";

            return RedirectToAction(nameof(Details), new { id = auctionId });
        }

        //---------------------------------------------------
        // MY BIDS
        //---------------------------------------------------

        public async Task<IActionResult> MyBids()
        {
            var buyer = await _userManager.GetUserAsync(User);

            if (buyer == null)
                return Challenge();

            var bids = await _bidService.GetBuyerBidsAsync(buyer.Id);

            return View(bids);
        }
    }
}
