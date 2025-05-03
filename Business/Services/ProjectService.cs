using Business.Extensions;
using Business.Interfaces;
using Business.Results;
using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.FormData;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public class ProjectService(IProjectRepository projectRepository, UserManager<UserEntity> userManager, INotificationService notificationService, AppDbContext context) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly INotificationService _notificationService = notificationService;
    private readonly AppDbContext _context = context;

    public async Task<ProjectResult> CreateProjectAsync(AddProjectFormData formdata)
    {
        if (formdata == null)
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Form data is required" };

        var entity = formdata.MapToEntity(); // Mappar 
        entity.StatusId = 4; // Sätter defaultstatus på alla nya projekt till "Started"

        // Kopplar användare till projektet via ID
        if (formdata.SelectedUserIds != null)
        {
            var validUserIds = await _userManager.Users
                .Where(u => formdata.SelectedUserIds.Contains(u.Id))
                .Select(u => u.Id)
                .ToListAsync();

            // Lägger till i många-många tabellen
            foreach (var userId in validUserIds)
            {
                entity.ProjectMembers.Add(new ProjectMemberEntity
                {
                    UserId = userId
                });
            }
        }

        var result = await _projectRepository.AddAsync(entity);
        if (!result.Succeeded)
            return new ProjectResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
        
        // Notifikationer
        await _notificationService.SendNotificationToMemberAsync
            ($"{entity.ProjectName} has been added to Projects",
            entity.ProjectImage!,
                2);

        return new ProjectResult { Succeeded = true, StatusCode = 201 };
    }

    public async Task<ProjectListResult> GetProjectsAsync(int page = 1, int pageSize = 4)
    {
        var entities = await _projectRepository.GetProjectsWithDetailsAsync();
        
        var paginatedProjects = entities
            .OrderByDescending(p => p.Created)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => p.MapToProject())
            .ToList();

        return new ProjectListResult { Succeeded = true, StatusCode = 200, Result = paginatedProjects };
    }

    public async Task<int> GetProjectsCountAsync()
    {
        var projects = await _projectRepository.GetProjectsWithDetailsAsync();
        return projects.Count();
    }
    
    public async Task<ProjectResult> GetProjectAsync(string id)
    {
        var response = await _projectRepository.GetAsync(
            where: x => x.Id == id,
            include => include.ProjectMembers,
            p => p.Client,
            p => p.Status);

        return response.Succeeded
            ? new ProjectResult { Succeeded = true, StatusCode = 200, Result = response.Result }
            : new ProjectResult { Succeeded = false, StatusCode = 404, Error = $"Project '{id}' was not found " };
    }

    public async Task<ProjectResult> UpdateProjectAsync(string id, UpdateProjectFormData formdata)
    {
        // Hämtar projekt & från ProjectMembers
        var projectResult = await _projectRepository.GetEntityAsync(
            x => x.Id == id,
            x => x.ProjectMembers
        );

        var entity = projectResult.Result;

        if (entity == null)
            return new ProjectResult { Succeeded = false, StatusCode = 404, Error = "Project not found" };

        // Uppdaterar
        formdata.UpdateEntity(entity);

        // Rensar gamla medlemmar och lägger till nya. Denna kodsnutt är genererad av Chat GPT 4.0.
        entity.ProjectMembers.Clear();

        foreach (var userId in formdata.SelectedUserIds)
        {
            entity.ProjectMembers.Add(new ProjectMemberEntity { ProjectId = entity.Id, UserId = userId });
        }

        var updateResult = await _projectRepository.UpdateAsync(entity);

        if (updateResult.Succeeded)
        {
            await _notificationService.SendNotificationToMemberAsync(
                $"{entity.ProjectName} has been updated",
                entity.ProjectImage!,
                2);
        }

        return new ProjectResult { Succeeded = updateResult.Succeeded, StatusCode = updateResult.StatusCode, Error = updateResult.Error };
    }
    public async Task<ProjectResult> DeleteProjectAsync(string id)
    {
        var entity = await _projectRepository.GetEntityAsync(x => x.Id == id);
        if (!entity.Succeeded || entity.Result == null)
            return new ProjectResult { Succeeded = false, StatusCode = 404, Error = "Project not found" };

        var deleteEntity = await _projectRepository.DeleteAsync(entity.Result);
        
        if (!entity.Succeeded)
        {
            await _notificationService.SendNotificationToMemberAsync(
                $"{entity.Result.ProjectName} has been deleted",
                entity.Result.ProjectImage!,
                2);
            
            return new ProjectResult { Succeeded = false, StatusCode = 500 };
        }

        return new ProjectResult { Succeeded = deleteEntity.Succeeded, StatusCode = deleteEntity.StatusCode, Error = deleteEntity.Error };
    }
    
    /* Denna kod är genererat av Chat GPT 4.0 för att uppdatera status på alla projekt
     baserat på datum */

    public async Task UpdateProjectStatusAsync()
    {
        var started = await _context.Statuses
            .Where(s => s.StatusName.ToLower() == "started")
            .Select(s => s.Id)
            .FirstOrDefaultAsync();
        
        var completed = await _context.Statuses
            .Where(s => s.StatusName.ToLower() == "completed")
            .Select(s => s.Id)
            .FirstOrDefaultAsync();
        
        var projects = await _context.Projects
            .Include(p => p.Status)
            .ToListAsync();

        foreach (var project in projects)
        {
            if (project.StartDate <= DateTime.Now && project.EndDate >= DateTime.Now)
            {
                project.StatusId = started;
            }

            if (project.EndDate < DateTime.Now)
            {
                project.StatusId = completed;
            }
        }
        
        await _context.SaveChangesAsync();
    }
}
    