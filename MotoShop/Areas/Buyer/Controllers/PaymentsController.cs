using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoShop.Data;
using MotoShop.Enums;
using MotoShop.Interfaces.Services;

namespace MotoShop.Areas.Buyer.Controllers
{
    [Area("Buyer")]
    [Authorize(Roles = "Buyer")]
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IInvoiceService _invoiceService;


        public PaymentsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager, IInvoiceService invoiceService)
        {
            _context = context;
            _userManager = userManager;
            _invoiceService = invoiceService;
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

        public async Task<IActionResult> DownloadReceipt(int id)
        {
            var buyer = await _userManager.GetUserAsync(User);

            var payment = await _context.Payments

                .Include(x => x.Buyer)

                .Include(x => x.Auction)
                    .ThenInclude(x => x.Vehicle)
                        .ThenInclude(x => x.VehicleBrand)

                .Include(x => x.Auction)
                    .ThenInclude(x => x.Vehicle)
                        .ThenInclude(x => x.VehicleModel)

                .Include(x => x.Auction)
                    .ThenInclude(x => x.Vehicle)
                        .ThenInclude(x => x.Seller)

                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.BuyerId == buyer.Id);

            if (payment == null)
                return NotFound();

            var pdf = _invoiceService.GenerateReceipt(payment);

            return File(
                pdf,
                "application/pdf",
                $"Receipt_{payment.Id}.pdf");
        }
    }
}
