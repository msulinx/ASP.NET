using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebbApplication.Extensions;
using WebbApplication.Models;

namespace WebbApplication.Controllers;

[Authorize(Roles = "Admin")]
[Route("admin/settings")]
public class AdminSettingsController(IStatusService statusService, IClientService clientService, IProjectService projectService) : Controller
{
    private IStatusService _statusService = statusService;
    private readonly IClientService _clientService = clientService;
    private IProjectService _projectService = projectService;

    public async Task<IActionResult> Index()
    {
        await _projectService.UpdateProjectStatusAsync();
        
        var statuses = await _statusService.GetStatusesWithProjectAsync();
        var clients = await _clientService.GetClientsAsync();

        var model = new AdminSettingsViewModel
        {
            Statuses = statuses.Result!.Select(s => s.MapToViewModel()).ToList(),
            Clients = clients.Result!.Select(c => c.MapToViewModel()).ToList(),
            AddStatusFormData = new(),
            AddClientFormData = new(),
            UpdateStatusFormData = new(),
            UpdateClientFormData = new(),
        };
        return View("Index", model);
    }
    
    // STATUS

    [HttpPost("add-status")]
    public async Task<IActionResult> AddStatus(AddStatusViewModel model)
    {
        if (!ModelState.IsValid)
            return View("Index", await GetSettingsViewModel(addStatus: model));
        
        var result = await _statusService.CreateStatusAsync(model.MapTo());

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.Error ?? "Failed to add status");
            return View("Index", await GetSettingsViewModel(addStatus: model));
        }
        return RedirectToAction("Index");
    }

    [HttpPost("update-status")]
    public async Task<IActionResult> UpdateStatus(UpdateStatusViewModel model)
    {
        if (!ModelState.IsValid)
            return View("Index", await GetSettingsViewModel(updateStatus: model));
    
        var result = await _statusService.UpdateStatusAsync(model.Id, model.MapTo());

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.Error ?? "Failed to update status");
            return View("Index", await GetSettingsViewModel(updateStatus: model));
        }

        return RedirectToAction("Index");
    }
    
    [HttpPost("delete-status/{id}")]
    public async Task<IActionResult> DeleteStatus(int id)
    {
        await _statusService.DeleteStatusAsync(id);
        return RedirectToAction("Index");
    }
    
    // CLIENTS

    [HttpPost("add-clients")]
    public async Task<IActionResult> AddClient(AddClientViewModel model)
    {
        if (!ModelState.IsValid)
            return View("Index", await GetSettingsViewModel(addClient: model));
        
        var result = await _clientService.CreateClientAsync(model.MapTo());

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.Error ?? "Failed to add client");
            return View("Index", await GetSettingsViewModel(addClient: model));
        }
        return RedirectToAction("Index");
    }
    
    [HttpPost("update-client")]
    public async Task<IActionResult> UpdateClient(UpdateClientViewModel model)
    {
        if (!ModelState.IsValid)
            return View("Index", await GetSettingsViewModel(updateClient: model));
        
        var result = await _clientService.UpdateClientAsync(model.Id, model.MapTo());

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.Error ?? "Failed to update client");
            return View("Index", await GetSettingsViewModel(updateClient: model));
        }
        
        return RedirectToAction("Index");
    }

    [HttpPost("delete-client/{id}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        await _clientService.DeleteClientAsync(id);
        return RedirectToAction("Index");
    }

    
    // Chat GPT
    private async Task<AdminSettingsViewModel> GetSettingsViewModel(
        AddClientViewModel? addClient = null,
        AddStatusViewModel? addStatus = null,
        UpdateClientViewModel? updateClient = null,
        UpdateStatusViewModel? updateStatus = null)
    {
        var statuses = await _statusService.GetStatusesWithProjectAsync();
        var clients = await _clientService.GetClientsAsync();

        return new AdminSettingsViewModel
        {
            Statuses = statuses.Result!.Select(s => s.MapToViewModel()).ToList(),
            Clients = clients.Result!.Select(c => c.MapToViewModel()).ToList(),
            AddStatusFormData = addStatus ?? new(),
            AddClientFormData = addClient ?? new(),
            UpdateStatusFormData = updateStatus ?? new(),
            UpdateClientFormData = updateClient ?? new(),
        };
    }
    
}