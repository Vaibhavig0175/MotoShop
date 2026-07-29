using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoShop.Data;
using MotoShop.Enums;

namespace MotoShop.Areas.Buyer.Controllers
{
    [Area("Buyer")]
    [Authorize(Roles = "Buyer")]
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
            var buyer = await _userManager.GetUserAsync(User);

            var payments = await _context.Payments
                .Include(p => p.Auction)
                    .ThenInclude(a => a.Vehicle)
                        .ThenInclude(v => v.VehicleBrand)
                .Include(p => p.Auction)
                    .ThenInclude(a => a.Vehicle)
                        .ThenInclude(v => v.VehicleModel)
                .Where(p => p.BuyerId == buyer.Id)
                .OrderByDescending(p => p.CreatedOn)
                .ToListAsync();

            return View(payments);
        }

        // GET
        public async Task<IActionResult> Pay(int id)
        {
            var buyer = await _userManager.GetUserAsync(User);

            var payment = await _context.Payments
                .Include(p => p.Auction)
                    .ThenInclude(a => a.Vehicle)
                        .ThenInclude(v => v.VehicleBrand)
                .Include(p => p.Auction)
                    .ThenInclude(a => a.Vehicle)
                        .ThenInclude(v => v.VehicleModel)
                .FirstOrDefaultAsync(p =>
                    p.Id == id &&
                    p.BuyerId == buyer.Id);

            if (payment == null)
                return NotFound();

            if (payment.Status != PaymentStatus.Pending)
            {
                TempData["Error"] = "Payment already completed.";
                return RedirectToAction(nameof(Index));
            }

            return View(payment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmPayment(int id)
        {
            var buyer = await _userManager.GetUserAsync(User);

            var payment = await _context.Payments
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.BuyerId == buyer.Id);

            if (payment == null)
                return NotFound();

            payment.Status = PaymentStatus.Paid;

            payment.PaymentDate = DateTime.Now;

            payment.TransactionId = "TXN-" + Guid.NewGuid().ToString("N")[..10].ToUpper();

            payment.UpdatedOn = DateTime.Now;

            _context.Update(payment);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Payment completed successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
