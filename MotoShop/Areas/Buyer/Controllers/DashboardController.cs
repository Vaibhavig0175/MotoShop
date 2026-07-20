using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoShop.Constants;

namespace MotoShop.Areas.Buyer.Controllers
{
    public class DashboardController : Controller
    {
        [Area("Buyer")]
        [Authorize(Roles = Roles.Buyer)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
