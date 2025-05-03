using Business.Interfaces;
using Business.Results;
using Data.Entities;
using Domain.FormData;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public class AuthenticationService(IUserService userService, UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, INotificationService notificationService, INotificationSender notificationSender) : IAuthenticationService
{
    private readonly IUserService _userService = userService;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly INotificationService _notificationService = notificationService;
    private readonly INotificationSender _notificationSender = notificationSender;
    
    public async Task<UserResult> SignUpAsync(SignUpFormData form)
    {
        var user = new UserEntity
        {
            UserName = form.Email,
            Email = form.Email,
            FirstName = form.FirstName,
            LastName = form.LastName
        };

        var result = await _userManager.CreateAsync(user, form.Password);

        return new UserResult
        {
            Succeeded = result.Succeeded,
            StatusCode = result.Succeeded ? 201 : 400,
            Error = result.Succeeded ? null : string.Join(", ", result.Errors.Select(e => e.Description))
        };
    }
    
    public async Task<UserResult> SignInAsync(SignInFormData form)
    {

        var user = await _userManager.FindByEmailAsync(form.Email);
        if (user == null)
        {
            return new UserResult
            {
                Succeeded = false,
                StatusCode = 401,
                Error = "User not found"
            };
        }

        var result = await _signInManager.PasswordSignInAsync(
            user.UserName,
            form.Password,
            form.RememberMe,
            lockoutOnFailure: false
        );

        if (result.Succeeded)
        {
            Console.WriteLine("🎉 Inloggning lyckades!");

            var notificationEntity = new NotificationEntity
            {
                Message = $"{user.FirstName} {user.LastName} signed in",
                NotificationTypeId = 1,
                TargetGroupId = 3,
                Icon = user.UserImage!,
            };

            await _notificationService.AddNotificationAsync(notificationEntity);
        }
        else
        {
            if (result.IsLockedOut) Console.WriteLine("⛔️ Konto är låst.");
            else if (result.IsNotAllowed) Console.WriteLine("⚠️ Inloggning är inte tillåten.");
            else Console.WriteLine("❌ Ogiltigt användarnamn eller lösenord.");
        }

        return new UserResult
        {
            Succeeded = result.Succeeded,
            StatusCode = result.Succeeded ? 200 : 401,
            Error = result.Succeeded ? null : "Invalid login attempt"
        };
    }
}