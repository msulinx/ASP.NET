using Business.Interfaces;
using Business.Results;
using Data.Entities;
using Data.Interfaces;
using Domain.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public class UserService(IUserRepository userRepository, UserManager<UserEntity> userManager, RoleManager<IdentityRole> roleManager) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    
    public async Task<UserResult> AddUserToRoleAsync(string userId, string roleName)
    {
     if (!await _roleManager.RoleExistsAsync(roleName))
         return new UserResult {Succeeded = false, StatusCode = 404, Error = "Role does not exist" };
     
     var user = await _userManager.FindByIdAsync(userId);
     if (user == null)
         return new UserResult {Succeeded = false, StatusCode = 404, Error = "User not found" };
     
     var result = await _userManager.AddToRoleAsync(user, roleName);
     return result.Succeeded
         ? new UserResult { Succeeded = true, StatusCode = 200 }
         : new UserResult { Succeeded = false, StatusCode = 500, Error = "Unable to add user to role" };
    }
    
    public async Task<UserResult> GetUsersAsync()
    {
        var result = await _userRepository.GetAllAsync();
        return result.MapTo<UserResult>();
    }

    public async Task<string> GetDisplayNameAsync(string? userName)
    {
        if (userName == null)
            return "";
        var user = await _userManager.FindByNameAsync(userName);
        
        return user == null ? "" : $"{user.FirstName} {user.LastName}";
    }
}