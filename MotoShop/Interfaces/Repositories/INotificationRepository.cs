using MotoShop.Models;

namespace MotoShop.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetUserNotificationsAsync(string userId);

        Task<List<Notification>> GetUnreadNotificationsAsync(string userId);

        Task<int> GetUnreadCountAsync(string userId);

        Task<Notification?> GetByIdAsync(int id);

        Task AddAsync(Notification notification);

        Task UpdateAsync(Notification notification);

        Task DeleteAsync(int id);

        Task MarkAsReadAsync(int id);

        Task MarkAllAsReadAsync(string userId);
    }
}
