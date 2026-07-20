using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoShop.Constants;

namespace MotoShop.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        [Area("Admin")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
