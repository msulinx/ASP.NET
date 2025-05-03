using Domain.Models;

namespace Business.Results;

public class ClientResult : ServiceResult
{
    public Client? Result { get; set; }
}