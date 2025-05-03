using Domain.Models;

namespace Business.Results;

public class ProjectListResult : ServiceResult
{
    public IEnumerable<Project>? Result { get; set; }
}