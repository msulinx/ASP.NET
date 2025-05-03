using Data.Entities;

namespace Business.Interfaces;

public interface INotificationService
{
    Task AddNotificationAsync(NotificationEntity entity, string userId = "anonymous");

    Task<IEnumerable<NotificationEntity>> GetNotificationsAsync(string userId, int take = 10);

    Task DismissNotificationAsync(string notificationId, string userId);

    Task SendNotificationToAdminAsync(string message, string icon, int notificationTypeId);

    Task SendNotificationToMemberAsync(string message, string icon, int notificationTypeId);
}