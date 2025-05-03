using Business.Results;
using Domain.FormData;
using Domain.Models;

namespace Business.Interfaces;

public interface IProjectService
{
    Task<ProjectResult> CreateProjectAsync(AddProjectFormData formdata);

    Task<ProjectListResult> GetProjectsAsync(int page = 1, int pageSize = 4);

    Task<int> GetProjectsCountAsync();

    Task<ProjectResult> GetProjectAsync(string id);
    
    Task<ProjectResult> UpdateProjectAsync(string id, UpdateProjectFormData formdata);
    
    Task<ProjectResult> DeleteProjectAsync(string id);

    Task UpdateProjectStatusAsync();
}