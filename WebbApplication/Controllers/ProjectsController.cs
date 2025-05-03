using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebbApplication.Extensions;
using WebbApplication.Models;

[Authorize]
[Route("projects")]
public class ProjectsController(
    IClientService clientService,
    IStatusService statusService,
    IMemberService memberService,
    IProjectService projectService) : Controller
{
    private readonly IProjectService _projectService = projectService;
    private readonly IMemberService _memberService = memberService;
    private readonly IClientService _clientService = clientService;
    private readonly IStatusService _statusService = statusService;
    public async Task<IActionResult> Index(string? status, int page = 1, int pageSize = 4)
    {
        var viewModel = await GetProjectsViewModel(
            addProject: null,
            status: status,
            page: page,
            pageSize: pageSize);
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> AddProject(AddProjectViewModel model)
    {
        if (!ModelState.IsValid)
            return View("Index", await GetProjectsViewModel(addProject: model));

        // Filuppladdning
        if (model.ProjectImage is { Length: > 0 })
        {
            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/projects");
            Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.ProjectImage.FileName)}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await model.ProjectImage.CopyToAsync(stream);

            model.ImageUrl = $"/uploads/projects/{fileName}";
        }

        var formData = model.MapToFormData();
        var result = await _projectService.CreateProjectAsync(formData);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.Error ?? "Failed to create project");
            return View("Index", await GetProjectsViewModel(addProject: model));
        }

        return RedirectToAction("Index");
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateProject(UpdateProjectViewModel model)
    {
        if (!ModelState.IsValid)
            return View("Index", await GetProjectsViewModel());

        if (model.ProjectImage is { Length: > 0 })
        {
            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/projects");
            Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.ProjectImage.FileName)}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await model.ProjectImage.CopyToAsync(stream);

            model.ImageUrl = $"/uploads/projects/{fileName}";
        }
        
        /*  Denna kodsnutt är genererad a Chat GPT 4.0.
         Om ingen ny bild är uppladdad och ingen gammal finns -> sätt inte till null */
        if (string.IsNullOrEmpty(model.ImageUrl))
        {
            var existingProject = await _projectService.GetProjectAsync(model.Id);
            if (existingProject.Succeeded && existingProject.Result != null)
            {
                model.ImageUrl = existingProject.Result.ImageUrl;
            }
        }

        var formData = model.MapToUpdateFormData();
        var result = await _projectService.UpdateProjectAsync(model.Id, formData);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.Error ?? "Failed to update project");
            return View("Index", await GetProjectsViewModel());
        }

        return RedirectToAction("Index");
    }

    [HttpPost("delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteProject(string id)
    {
        await _projectService.DeleteProjectAsync(id);
        return RedirectToAction("Index");
    }

    private async Task<IEnumerable<SelectListItem>> GetClientsAsync()
    {
        var result = await _clientService.GetClientsAsync();
        return result.Result.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.ClientName
        });
    }

    private async Task<IEnumerable<SelectListItem>> GetStatusesAsync()
    {
        var result = await _statusService.GetStatusesAsync();
        return result.Result.Select(s => new SelectListItem
        {
            Value = s.Id.ToString(),
            Text = s.StatusName
        });
    }

    private async Task<List<MemberViewModel>> GetAllMembersAsync()
    {
        var result = await _memberService.GetMembersAsync();
        return result.Result.Select(m => new MemberViewModel
        {
            Id = m.Id,
            FirstName = m.FirstName,
            LastName = m.LastName,
            ImageUrl = m.ImageUrl
        }).ToList();
    }

    private async Task<ProjectsIndexViewModel> GetProjectsViewModel(
        AddProjectViewModel? addProject = null,
        string? status = null,
        int page = 1,
        int pageSize = 4)
    {
        await _projectService.UpdateProjectStatusAsync();

        // Hämtar paginerade projekt
        var projectsResult = await _projectService.GetProjectsAsync(page, pageSize);
        var allProjectsResult = await _projectService.GetProjectsAsync(); // Hämta ALLA projekt för statistik

        var clients = await GetClientsAsync();
        var statuses = await GetStatusesAsync();
        var allMembers = await GetAllMembersAsync();

        var projects = projectsResult.Result!.ToList();
        var allProjects = allProjectsResult.Result!.ToList();

        // Beräkna totalprojektsiffror
        var totalProjects = await _projectService.GetProjectsCountAsync();
        var totalPages = (int)Math.Ceiling(totalProjects / (double)pageSize);

        ViewBag.ProjectCountAll = allProjects.Count;
        ViewBag.ProjectCountStarted = allProjects.Count(p => p.Status?.ToLower() == "started");
        ViewBag.ProjectCountCompleted = allProjects.Count(p => p.Status?.ToLower() == "completed");
        ViewBag.SelectedStatus = status ?? "all";

        return new ProjectsIndexViewModel
        {
            Projects = projects
                .Select(p => p.MapToViewModel())
                .ToList(),

            AddProjectFormData = addProject ?? new AddProjectViewModel
            {
                Clients = clients,
                Members = allMembers
            },

            UpdateProjectViewModels = projects
                .Select(p => p.MapToUpdateViewModel(clients, allMembers, statuses))
                .ToList(),

            CurrentPage = page,
            PageSize = pageSize,
            TotalPages = totalPages
        };
    }
}