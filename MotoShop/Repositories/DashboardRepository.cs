using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MotoShop.Areas.Admin.Models;
using MotoShop.Data;
using MotoShop.Interfaces.Repositories;

namespace MotoShop.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardRepository(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<DashboardViewModel> GetDashboardAsync()
        {
            var roleCounts = await (
    from userRole in _context.UserRoles
    join role in _context.Roles
        on userRole.RoleId equals role.Id
    group userRole by role.Name into g
    select new
    {
        Role = g.Key,
        Count = g.Count()
    }
).ToDictionaryAsync(x => x.Role!, x => x.Count);


            return new DashboardViewModel
            {
                TotalVehicles = await _context.Vehicles.CountAsync(),

                LiveAuctions = 0,

                TotalBuyers = roleCounts.GetValueOrDefault("Buyer", 0),

                TotalSellers = roleCounts.GetValueOrDefault("Seller", 0),

                Revenue = 0,

                PendingApprovals = 0,

                RecentVehicles = await _context.Vehicles
                    .Include(x => x.VehicleBrand)
                    .Include(x => x.VehicleModel)
                    .Include(x => x.Seller)
                    .OrderByDescending(x => x.CreatedOn)
                    .Take(10)
                    .Select(x => new RecentVehicleVM
                    {
                        VehicleName = x.VehicleBrand.Name + " " + x.VehicleModel.Name,
                        SellerName = x.Seller.FullName,
                        CreatedOn = x.CreatedOn
                    })
                    .ToListAsync()
            };
        }
    }
}
