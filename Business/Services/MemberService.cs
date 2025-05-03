using Business.Extensions;
using Business.Interfaces;
using Business.Results;
using Data.Entities;
using Data.Interfaces;
using Domain.Extensions;
using Domain.FormData;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public class MemberService(IUserRepository userRepository, INotificationService notificationService, UserManager<UserEntity> userManager) : IMemberService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly INotificationService _notificationService = notificationService;
    private readonly UserManager<UserEntity> _userManager = userManager;
    
public async Task<UserResult> CreateMemberAsync(AddMemberFormData form)
{
    var existingUser = await _userManager.FindByEmailAsync(form.Email!);
    if (existingUser == null)
        return new UserResult { Succeeded = false, StatusCode = 404, Error = "User not found" };
    
    form.MapToEntity(existingUser);

    var updateResult = await _userManager.UpdateAsync(existingUser);
    if (!updateResult.Succeeded)
        return new UserResult { Succeeded = false, StatusCode = 400, Error = $"Failed to update user '{existingUser.Email}':"};
    
    
    // Kontrollerar rollen på användare, om roll inte finns, lägg till
    var roleExists = await _userManager.IsInRoleAsync(existingUser, form.Role!);
    if (!roleExists)
    {
        var roleResult = await _userManager.AddToRoleAsync(existingUser, form.Role!);
        if (!roleResult.Succeeded)
            return new UserResult { Succeeded = false, StatusCode = 400, Error = "Failed to assign role" };
    }

    // Notifikationer som bara skickas till Admin
    await _notificationService.SendNotificationToAdminAsync(
        $"{existingUser.FirstName} {existingUser.LastName} added to Member",
        existingUser.UserImage!,
        3
    );

    return new UserResult { Succeeded = true, StatusCode = 200 };
}

public async Task<UserListResult> GetMembersAsync(int page = 1, int pageSize = 6)
{
    var members = await _userManager.GetUsersInRoleAsync("Member");
    var paginatedMembers = members
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(m => m.MapToMember())
        .ToList();

    return new UserListResult { Succeeded = true, StatusCode = 200, Result = paginatedMembers };
        
}

public async Task<int> GetMembersCountAsync()
{
    var members = await _userManager.GetUsersInRoleAsync("Member");
    return members.Count;
}

public async Task<UserResult> GetMemberByIdAsync(string id)
{
    var result = await _userRepository.GetAsync(x => x.Id == id);
    return result.MapTo<UserResult>();
}

    
    public async Task<UserResult> UpdateMemberAsync(string id, UpdateMemberFormData form)
    {
        var response = await _userRepository.GetEntityAsync(x => x.Id == id);
        var entity = response.Result;

        if (entity == null)
            return new UserResult { Succeeded = false, StatusCode = 404, Error = "Member not found" };
        
        form.UpdateEntity(entity);

        var updateResult = await _userRepository.UpdateAsync(entity);
        
        if (updateResult.Succeeded)
        {
            await _notificationService.SendNotificationToAdminAsync(
                $"{entity.FirstName} {entity.LastName} has been updated",
                entity.UserImage!,
                3
            );
        }
        
        return updateResult.Succeeded 
            ? new UserResult { Succeeded = true, StatusCode = updateResult.StatusCode } 
            : new UserResult { Succeeded = false, StatusCode = updateResult.StatusCode, Error = updateResult.Error };
    }

    public async Task<UserResult> DeleteMemberAsync(string id)
    {
        var response = await _userRepository.GetEntityAsync(x => x.Id == id);
        var entity = response.Result;

        if (entity == null)
            return new UserResult { Succeeded = false, StatusCode = 404, Error = "Member not found" };

        var deleteResult = await _userRepository.DeleteAsync(entity);
        
        if (deleteResult.Succeeded)
        {
            await _notificationService.SendNotificationToAdminAsync(
                $"{entity.FirstName} {entity.LastName} has been deleted",
                entity.UserImage!,
                3
            );
        }

        return new UserResult { Succeeded = deleteResult.Succeeded, StatusCode = deleteResult.StatusCode, Error = deleteResult.Error };
    }
}