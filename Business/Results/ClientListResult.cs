using Domain.Models;

namespace Business.Results;

public class ClientListResult : ServiceResult
{
    public IEnumerable<Client>? Result { get; set; }
}