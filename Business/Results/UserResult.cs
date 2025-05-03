using Domain.Models;

namespace Business.Results;

public class UserResult : ServiceResult
{
    public Member? Result { get; set; }
}