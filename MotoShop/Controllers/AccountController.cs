using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MotoShop.Constants;
using MotoShop.Data;
using MotoShop.Models;
using MotoShop.ViewModels.Account;

namespace MotoShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
               ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult RegisterBuyer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterBuyer(RegisterBuyerViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Buyer);

                await _signInManager.SignInAsync(user, false);

                return RedirectToAction("Index", "Dashboard", new { area = "Buyer" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult RegisterSeller()
        {
            var model = new RegisterSellerViewModel
            {
                States = GetIndianStates()
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterSeller(RegisterSellerViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            model.States = GetIndianStates();
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }

            await _userManager.AddToRoleAsync(user, Roles.Seller);

            var profile = new SellerProfile
            {
                UserId = user.Id,
                BusinessName = model.BusinessName,
                GstNumber = model.GstNumber,
                BusinessAddress = model.BusinessAddress,
                City = model.City,
                State = model.State,
                Pincode = model.Pincode
            };

            _context.SellerProfiles.Add(profile);
            await _context.SaveChangesAsync();

            await _signInManager.SignInAsync(user, false);

            return RedirectToAction("Index", "Dashboard", new { area = "Seller" });
        }

        private List<SelectListItem> GetIndianStates()
        {
            return IndianStates.States
                .Select(state => new SelectListItem
                {
                    Value = state,
                    Text = state
                })
                .ToList();
        }
    }
}
