using Domain.Models;

namespace Business.Results;

public class StatusResult : ServiceResult
{
    public Status? Result { get; set; }
}