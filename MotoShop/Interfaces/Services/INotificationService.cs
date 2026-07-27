using MotoShop.Models;

namespace MotoShop.Interfaces.Services
{
    public interface INotificationService
    {
        Task<List<Notification>> GetUserNotificationsAsync(string userId);

        Task<List<Notification>> GetUnreadNotificationsAsync(string userId);

        Task<int> GetUnreadCountAsync(string userId);

        Task<Notification?> GetByIdAsync(int id);

        Task CreateNotificationAsync(
            string userId,
            string title,
            string message,
            NotificationType type,
            string? url = null);

        Task MarkAsReadAsync(int id);

        Task MarkAllAsReadAsync(string userId);

        Task DeleteAsync(int id);
    }
}
