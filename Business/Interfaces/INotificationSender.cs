namespace Business.Interfaces;

public interface INotificationSender
{
    Task SendNotificationAsync(string userId, string message);
}