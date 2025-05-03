using Domain.Models;

namespace Business.Results;

public class ProjectResult : ServiceResult
{
    public Project? Result { get; set; }
}