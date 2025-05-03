using Domain.Models;

namespace Business.Results;

public class StatusListResult : ServiceResult
{
    public IEnumerable<Status>? Result { get; set; }
}