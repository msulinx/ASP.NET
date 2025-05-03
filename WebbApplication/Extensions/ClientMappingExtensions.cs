using Domain.FormData;
using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebbApplication.Models;

namespace WebbApplication.Extensions;

public static class ClientMappingExtensions
{

    // Viewmodel --> Add Formdata
    public static AddClientFormData MapTo(this AddClientViewModel model)
    {
        return new AddClientFormData
        {
            ClientName = model.ClientName,
            Email = model.Email,
        };
    }

    // Viewmodel --> Update Formdata
    public static UpdateClientFormData MapTo(this UpdateClientViewModel model)
    {
        return new UpdateClientFormData
        {
            Id = model.Id,
            ClientName = model.ClientName,
            Email = model.Email,
        };
    }
    
    // Client --> Viewmodel
    public static ClientViewModel MapToViewModel(this Client client)
    {
        return new ClientViewModel
        {
            Id = client.Id,
            ClientName = client.ClientName,
            Email = client.Email
        };
    }
}