using Domain.Models;

namespace Business.Results;

public class UserListResult : ServiceResult
{
    public IEnumerable<Member>? Result { get; set; }
}