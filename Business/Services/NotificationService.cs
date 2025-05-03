using Business.Interfaces;
using Data.Contexts;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public class NotificationService(AppDbContext context, INotificationSender notificationSender, UserManager<UserEntity> userManager) : INotificationService
{
    private readonly AppDbContext _context = context;
    private readonly INotificationSender _notificationSender = notificationSender;
    private readonly UserManager<UserEntity> _userManager = userManager;

    public async Task AddNotificationAsync(NotificationEntity entity, string userId = "anonymous")
    {
        if (string.IsNullOrEmpty(entity.Icon))
        {
            switch (entity.NotificationTypeId)
            {
                case 1:
                    entity.Icon = "user";
                    break;
                case 2:
                    entity.Icon = "project";
                    break;
                case 3:
                    entity.Icon = "member";
                    break;
            }
        }
        
        _context.Add(entity);
        await _context.SaveChangesAsync();
        
        var notifications = await GetNotificationsAsync(userId);
        var newNotification = notifications.OrderByDescending(x => x.Created).FirstOrDefault();
        if (newNotification != null)
        {
            await _notificationSender.SendNotificationAsync(userId, newNotification.Message);
        }
    }

    public async Task<IEnumerable<NotificationEntity>> GetNotificationsAsync(string userId, int take = 10)
    {
        var dismissedIds = await _context.NotificationDismisses.Where(x => x.UserId == userId)
            .Select(x => x.NotificationId)
            .ToListAsync();
        
        var notifications = await _context.Notifications
            .Where(x => !dismissedIds.Contains(x.Id))
            .OrderByDescending(x => x.Created)
            .Take(take)
            .ToListAsync();
        
        return notifications;
    }

    public async Task DismissNotificationAsync(string notificationId, string userId)
    {
        var alreadyDismissed = await _context.NotificationDismisses.AnyAsync(x => x.NotificationId == notificationId && x.UserId == userId);
        if (!alreadyDismissed)
        {
            var dismissed = new NotificationDismissEntity
            {
                NotificationId = notificationId,
                UserId = userId
            };
            _context.Add(dismissed);
            await _context.SaveChangesAsync();
        }
    }
    
    /* 2 notifikationsmetoder är skapade för att slippa duplicering av kod i serviceklasserna */

    public async Task SendNotificationToAdminAsync(string message, string icon, int notificationTypeId)
    {
        var admins = await _userManager.GetUsersInRoleAsync("Admin");

        foreach (var admin in admins)
        {
            var notification = new NotificationEntity
            {
                Message = message,
                Icon = icon,
                NotificationTypeId = notificationTypeId,
                TargetGroupId = 2
            };
            
            await AddNotificationAsync(notification, admin.Id);
        }
    }

    public async Task SendNotificationToMemberAsync(string message, string icon, int notificationTypeId)
    {
        var members = await _userManager.GetUsersInRoleAsync("Member");

        foreach (var member in members)
        {
            var notification = new NotificationEntity
            {
                Message = message,
                Icon = icon,
                NotificationTypeId = notificationTypeId,
                TargetGroupId = 3
            };
            
            await AddNotificationAsync(notification, member.Id);
        }
    }
}