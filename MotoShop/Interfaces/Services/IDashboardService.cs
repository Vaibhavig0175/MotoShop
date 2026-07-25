using MotoShop.Areas.Admin.Models;

namespace MotoShop.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardAsync();
    }
}
