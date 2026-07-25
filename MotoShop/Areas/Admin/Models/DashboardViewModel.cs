namespace MotoShop.Areas.Admin.Models
{
    public class DashboardViewModel
    {
        public int TotalVehicles { get; set; }

        public int LiveAuctions { get; set; }

        public int TotalBuyers { get; set; }

        public int TotalSellers { get; set; }

        public decimal Revenue { get; set; }

        public int PendingApprovals { get; set; }

        public List<RecentVehicleVM> RecentVehicles { get; set; } = [];

        public List<RecentUserVM> RecentBuyers { get; set; } = [];

        public List<RecentUserVM> RecentSellers { get; set; } = [];
    }
}
