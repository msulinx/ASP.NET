using Business.Results;
using Domain.FormData;

namespace Business.Interfaces;

public interface IAuthenticationService
{
    Task<UserResult> SignUpAsync(SignUpFormData form);

    Task<UserResult> SignInAsync(SignInFormData form);
}