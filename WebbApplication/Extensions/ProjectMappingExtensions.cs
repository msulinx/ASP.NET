
using Domain.FormData;
using Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebbApplication.Models;

namespace WebbApplication.Extensions;

public static class ProjectMappingExtensions
{
    // Viewmodel --> Formdata
    
    public static AddProjectFormData MapToFormData(this AddProjectViewModel model)
    {
        return new AddProjectFormData
        {
            ProjectName = model.ProjectName,
            Description = model.RichTextContent,
            ImageUrl = model.ImageUrl,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Budget = model.Budget,
            SelectedClientId = model.SelectedClientId,
            SelectedUserIds = model.SelectedUserIds ?? new List<string>()
        };
    }
    
    // Project --> Viewmodel
    public static ProjectViewModel MapToViewModel(this Project project)
    {
        return new ProjectViewModel
        {
            Id = project.Id,
            ProjectName = project.ProjectName,
            ImageUrl = project.ImageUrl,
            Description = project.Description!,
            ClientName = project.ClientName,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Status = project.Status,
            Created = project.Created,
            Members = project.Members.Select(m => new MemberViewModel
            {
                Id = m.Id,
                FirstName = m.FirstName,
                LastName = m.LastName,
                ImageUrl = m.ImageUrl,
            }).ToList()
        };
    }
    
    // Viewmodel --> Formdata
    
    public static UpdateProjectFormData MapToUpdateFormData(this UpdateProjectViewModel model)
    {
        return new UpdateProjectFormData
        {
            Id = model.Id,
            ImageUrl = model.ImageUrl,
            ProjectName = model.ProjectName,
            ClientId = model.ClientId,
            SelectedUserIds = model.SelectedUserIds ?? new List<string>(),
            Description = model.RichTextContent,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Budget = model.Budget,
            StatusId = model.StatusId
        };
    }

    /* Denna kod är genererad av Chat GPT 4.0 för att visa sparade uppgifter
     i updatemodalen. I detta fall tidigare valda clients, members och status som är
     kopplade till existerande projekt */
    public static UpdateProjectViewModel MapToUpdateViewModel(this
            Project project, IEnumerable<SelectListItem> clients,
        List<MemberViewModel> members,
        IEnumerable<SelectListItem> statuses)
    {
        return new UpdateProjectViewModel
        {
            Id = project.Id,
            ProjectName = project.ProjectName,
            ImageUrl = project.ImageUrl,
            RichTextContent = project.Description,
            StartDate = project.StartDate ?? DateTime.Now,
            EndDate = project.EndDate ?? DateTime.Now,
            Budget = project.Budget ?? 0m,
            SelectedUserIds = project.Members.Select(m => m.Id).ToList(),
            Clients = clients,
            ClientId = project.ClientId,
            Statuses = statuses,
            Members = members,
        };
    }
    
    
}