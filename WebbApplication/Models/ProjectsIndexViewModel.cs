namespace WebbApplication.Models;

public class ProjectsIndexViewModel
{
    public List<ProjectViewModel> Projects { get; set; } = new();

    public AddProjectViewModel AddProjectFormData { get; set; } = new();

    public List<UpdateProjectViewModel> UpdateProjectViewModels { get; set; } = new();
    
    public int CurrentPage { get; set; } 
    public int PageSize { get; set; }

    public List<int> PageSizeOptions { get; set; } = new() { 2, 5, 8 };
    public int TotalPages { get; set; }
}