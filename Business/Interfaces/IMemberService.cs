using Business.Results;
using Domain.FormData;

namespace Business.Interfaces;

public interface IMemberService
{
    Task<UserListResult> GetMembersAsync(int page = 1, int pageSize = 6);

    Task<int> GetMembersCountAsync();

    Task<UserResult> GetMemberByIdAsync(string id);

    Task<UserResult> CreateMemberAsync(AddMemberFormData form);

    Task<UserResult> UpdateMemberAsync(string id, UpdateMemberFormData form);

    Task<UserResult> DeleteMemberAsync(string id);
}