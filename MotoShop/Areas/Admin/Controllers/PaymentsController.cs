using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoShop.Data;
using MotoShop.Enums;
using MotoShop.Interfaces.Services;

namespace MotoShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IInvoiceService _invoiceService;

        public PaymentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,IInvoiceService invoiceService)
        {
            _context = context;
            _userManager = userManager;
            _invoiceService = invoiceService;
        }

        public async Task<IActionResult> Index(string search)
        {
            var payments = _context.Payments
                .Include(p => p.Buyer)
                .Include(p => p.Auction)
                    .ThenInclude(a => a.Vehicle)
                        .ThenInclude(v => v.VehicleBrand)
                .Include(p => p.Auction)
                    .ThenInclude(a => a.Vehicle)
                        .ThenInclude(v => v.VehicleModel)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                payments = payments.Where(x =>
                    x.Buyer.FullName.Contains(search) ||
                    x.TransactionId!.Contains(search));
            }

            ViewBag.TotalPayments = await payments.CountAsync();

            ViewBag.Pending = await payments.CountAsync(x =>
                x.Status == PaymentStatus.Paid);

            ViewBag.Verified = await payments.CountAsync(x =>
                x.Status == PaymentStatus.Verified);

            ViewBag.Refunded = await payments.CountAsync(x =>
                x.Status == PaymentStatus.Refunded);

            return View(await payments
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync());
        }

        public async Task<IActionResult> Verify(int id)
        {
            var payment = await _context.Payments.FindAsync(id);

            if (payment == null)
                return NotFound();

            payment.Status = PaymentStatus.Verified;

            payment.UpdatedOn = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Payment verified successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Reject(int id)
        {
            var payment = await _context.Payments.FindAsync(id);

            if (payment == null)
                return NotFound();

            payment.Status = PaymentStatus.Failed;

            payment.UpdatedOn = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Payment rejected.";

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Release(int id)
        {
            var payment = await _context.Payments
                .Include(x => x.Auction)
                .ThenInclude(x => x.Vehicle)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (payment == null)
                return NotFound();

            payment.IsSettled = true;

            payment.SettlementDate = DateTime.Now;

            payment.SettledById = _userManager.GetUserId(User);

            payment.UpdatedOn = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Settlement released.";

            return RedirectToAction(nameof(Index));
        }
    }
}
