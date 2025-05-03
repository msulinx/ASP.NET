using Domain.FormData;
using WebbApplication.Models;

namespace WebbApplication.Extensions;

public static class SignUpMappingExtensions
{
    // Viewmodel --> Formdata
    public static SignUpFormData MapTo(this SignUpViewModel model)
    {
        return new SignUpFormData
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Password = model.Password,
            Role = model.Role,
        };
    }
}