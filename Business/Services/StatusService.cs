using Business.Interfaces;
using Business.Results;
using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Extensions;
using Domain.FormData;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public class StatusService(IStatusRepository statusRepository, UserManager<UserEntity> userManager, INotificationService notificationService, AppDbContext context) : IStatusService
{
    private readonly IStatusRepository _statusRepository = statusRepository;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly INotificationService _notificationService = notificationService;
    private readonly AppDbContext _context = context;
    

    public async Task<StatusResult> CreateStatusAsync(AddStatusFormData form)
    {
        var entity = form.MapTo<StatusEntity>(); 
        
        var result = await _statusRepository.AddAsync(entity);
        
        if (result.Succeeded)
        {
            await _notificationService.SendNotificationToAdminAsync(
                $"{entity.StatusName} has been added to Status",
                "status",
                5
            );
        }
        
        return new StatusResult { Succeeded = result.Succeeded, StatusCode = result.StatusCode, Error = result.Error };
    }
    
    public async Task<StatusListResult> GetStatusesAsync()
    {
        var statuses = await _statusRepository.GetAllAsync();
        
        return new StatusListResult { Succeeded = true, StatusCode = 200, Result = statuses.Result };
    }

    // För statussidan som håller koll på hur många projekt varje status har
    public async Task<StatusListResult> GetStatusesWithProjectAsync()
    {
        var statusList = await _statusRepository.GetAllStatusesWithProjectsAsync();

        return new StatusListResult { Succeeded = true, Result = statusList };
    }

    public async Task<StatusResult> UpdateStatusAsync(int id, UpdateStatusFormData form)
    {
        var existing = await _statusRepository.GetEntityAsync(x => x.Id == id);
        var entity = existing.Result;
        if (!existing.Succeeded || existing.Result == null)
            return new StatusResult { Succeeded = false, StatusCode = 404, Error = "Client not found" };
        
        entity!.StatusName = form.StatusName;

        var updateResult = await _statusRepository.UpdateAsync(entity);
        
        if (updateResult.Succeeded)
        {
            await _notificationService.SendNotificationToAdminAsync(
                $"{entity.StatusName} has been updated",
                "status",
                5
            );
        }

        return new StatusResult { Succeeded = updateResult.Succeeded, StatusCode = updateResult.StatusCode, Error = updateResult.Error };
    }

    public async Task<StatusResult> DeleteStatusAsync(int id)
    {
        var response = await _statusRepository.GetEntityAsync(x => x.Id == id);
        var entity = response.Result;

        if (entity == null)
            return new StatusResult { Succeeded = false, StatusCode = 404, Error = "Member not found" };

        var deleteResult = await _statusRepository.DeleteAsync(entity);
        
        if (deleteResult.Succeeded)
        {
            await _notificationService.SendNotificationToAdminAsync(
                $"{entity.StatusName} has been deleted from Status",
                "status",
                5
            );
        }

        return new StatusResult { Succeeded = deleteResult.Succeeded, StatusCode = deleteResult.StatusCode, Error = deleteResult.Error };
    }
}