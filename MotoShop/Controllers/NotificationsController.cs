using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MotoShop.Data;
using MotoShop.Interfaces.Services;

namespace MotoShop.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationsController(
            INotificationService notificationService,
            UserManager<ApplicationUser> userManager)
        {
            _notificationService = notificationService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var notifications =
                await _notificationService
                    .GetUserNotificationsAsync(user!.Id);

            return View(notifications);
        }

        public async Task<IActionResult> MarkRead(int id)
        {
            await _notificationService.MarkAsReadAsync(id);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MarkAllRead()
        {
            var user = await _userManager.GetUserAsync(User);

            await _notificationService
                .MarkAllAsReadAsync(user!.Id);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _notificationService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
