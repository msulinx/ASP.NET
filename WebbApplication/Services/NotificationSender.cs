using Business.Interfaces;
using Microsoft.AspNetCore.SignalR;
using WebbApplication.Hubs;

namespace WebbApplication.Services;

/* Denna klass är genererad av Chat GPT 4.0 för att agera som en mellanklass
 mellan applikation och business lagret för att kunna skapa notiser från 
 serviceklasserna och skicka de tillbaka till vyn */

public class NotificationSender(IHubContext<NotificationHub> hubContext) : INotificationSender
{
    public async Task SendNotificationAsync(string userId, string message)
    {
        await hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message);
    }
}