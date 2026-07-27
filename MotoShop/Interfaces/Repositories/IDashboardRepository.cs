using MotoShop.Areas.Admin.Models;

namespace MotoShop.Interfaces.Repositories
{
    public interface IDashboardRepository
    {
        Task<DashboardViewModel> GetDashboardAsync();
    }
}
