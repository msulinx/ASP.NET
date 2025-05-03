using Business.Results;

namespace Business.Interfaces;

public interface IUserService
{
    Task<UserResult> AddUserToRoleAsync(string userId, string roleName);
    Task<UserResult> GetUsersAsync();

    Task<string> GetDisplayNameAsync(string? userName);
}