using Business.Results;
using Domain.FormData;

namespace Business.Interfaces;

public interface IClientService
{
    Task<ClientResult> CreateClientAsync(AddClientFormData form);

    Task<ClientListResult> GetClientsAsync();

    Task<ClientResult> GetClientByIdAsync(int id);

    Task<ClientResult> UpdateClientAsync(int id, UpdateClientFormData form);

    Task<ClientResult> DeleteClientAsync(int id);
}