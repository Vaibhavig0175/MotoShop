using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MotoShop.Areas.Admin.Models;
using MotoShop.Data;
using MotoShop.Interfaces.Repositories;
using MotoShop.Interfaces.Services;

namespace MotoShop.Services.Admin
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repository;

        public DashboardService(IDashboardRepository repository)
        {
            _repository = repository;
        }

        public async Task<DashboardViewModel> GetDashboardAsync()
        {
            return await _repository.GetDashboardAsync();
        }
    }
}
