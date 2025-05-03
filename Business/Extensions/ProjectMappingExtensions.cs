using Data.Entities;
using Domain.FormData;
using Domain.Models;

namespace Business.Extensions;

public static class ProjectMappingExtensions
{
    
    // AddProjectFormData --> ProjectEntity
    public static ProjectEntity MapToEntity(this AddProjectFormData form)
    {
        return new ProjectEntity
        {
            Id = Guid.NewGuid().ToString(),
            ProjectName = form.ProjectName,
            Description = form.Description,
            ProjectImage = form.ImageUrl,
            StartDate = form.StartDate,
            EndDate = form.EndDate,
            Budget = form.Budget,
            ClientId = form.SelectedClientId,
            StatusId = 4, // Default status
            Created = DateTime.Now,
            ProjectMembers = new List<ProjectMemberEntity>() // Lista för members
        };
    }
    
    // UpdateProjectFormData --> ProjectEntity
    public static void UpdateEntity (this UpdateProjectFormData form, ProjectEntity entity)
    {
        entity.ProjectName = form.ProjectName;
        entity.Description = form.Description;
        entity.ProjectImage = form.ImageUrl;
        entity.StartDate = form.StartDate;
        entity.EndDate = form.EndDate;
        entity.Budget = form.Budget;
        entity.ClientId = form.ClientId;
        entity.StatusId = form.StatusId;
    }
    
    // ProjectEntity --> Project
    
    /* Kodraderna för ClientName, Status och Members har tagits hjälp från Chat GPT.
     För Client och Status skickas en tom sträng till vyn ifall det inte finns något
     kopplat till projektet. För Members skickas Id, namn och avatarbild från entiteten */
    public static Project MapToProject(this ProjectEntity entity)
    {
        return new Project
        {
            Id = entity.Id,
            ProjectName = entity.ProjectName,
            ImageUrl = entity.ProjectImage,
            Description = entity.Description,
            ClientName = entity.Client?.ClientName ?? "",
            ClientId = entity.ClientId,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Budget = entity.Budget,
            Status = entity.Status?.StatusName ?? "",
            Members = entity.ProjectMembers.Select(pm => new Member
            {
                Id = pm.UserId,
                FirstName = pm.Member.FirstName!,
                LastName = pm.Member.LastName!,
                ImageUrl = pm.Member.UserImage
            }).ToList()
        };

    }

}