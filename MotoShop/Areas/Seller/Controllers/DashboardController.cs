using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoShop.Constants;

namespace MotoShop.Areas.Seller.Controllers
{
    public class DashboardController : Controller
    {
        [Area("Seller")]
        [Authorize(Roles = Roles.Seller)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
