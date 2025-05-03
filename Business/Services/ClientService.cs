using Business.Interfaces;
using Business.Results;
using Data.Entities;
using Data.Interfaces;
using Domain.Extensions;
using Domain.FormData;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public class ClientService(IClientRepository clientRepository, INotificationService notificationService, UserManager<UserEntity> userManager) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly INotificationService _notificationService = notificationService;
    private readonly UserManager<UserEntity> _userManager = userManager;

    public async Task<ClientResult> CreateClientAsync(AddClientFormData form)
    {
        var entity = form.MapTo<ClientEntity>(); 

        var result = await _clientRepository.AddAsync(entity);
        
        if (result.Succeeded)
        {
            await _notificationService.SendNotificationToAdminAsync(
                $"{entity.ClientName} has been added to Clients",
                "client",
                4
            );
        }

        return new ClientResult { Succeeded = result.Succeeded, StatusCode = result.StatusCode, Error = result.Error };
    }
    
    public async Task<ClientListResult> GetClientsAsync()
    {
        var clients = await _clientRepository.GetAllAsync();
        
        return new ClientListResult { Succeeded = true, StatusCode = 200, Result = clients.Result };
    }
    
    public async Task<ClientResult> GetClientByIdAsync(int id)
    {
        var result = await _clientRepository.GetAsync(x => x.Id == id);
        return result.MapTo<ClientResult>();
    }

    public async Task<ClientResult> UpdateClientAsync(int id, UpdateClientFormData form)
    {
        var existing = await _clientRepository.GetEntityAsync(x => x.Id == id);
        var entity = existing.Result;
        if (!existing.Succeeded || existing.Result == null)
            return new ClientResult { Succeeded = false, StatusCode = 404, Error = "Client not found" };
        
        entity!.ClientName = form.ClientName;
        entity.Email = form.Email;

        var updateResult = await _clientRepository.UpdateAsync(entity);
        
        if (updateResult.Succeeded)
        {
            await _notificationService.SendNotificationToAdminAsync(
                $"{entity.ClientName} has been updated",
                "client",
                4
            );
        }

        return new ClientResult { Succeeded = updateResult.Succeeded, StatusCode = updateResult.StatusCode, Error = updateResult.Error };
    }

    public async Task<ClientResult> DeleteClientAsync(int id)
    {
        var response = await _clientRepository.GetEntityAsync(x => x.Id == id);
        var entity = response.Result;

        if (entity == null)
            return new ClientResult { Succeeded = false, StatusCode = 404, Error = "Member not found" };

        var deleteResult = await _clientRepository.DeleteAsync(entity);
        
        if (deleteResult.Succeeded)
        {
            await _notificationService.SendNotificationToAdminAsync(
                $"{entity.ClientName} has been deleted",
                "client",
                4
            );
        }

        return new ClientResult { Succeeded = deleteResult.Succeeded, StatusCode = deleteResult.StatusCode, Error = deleteResult.Error };
    }
}