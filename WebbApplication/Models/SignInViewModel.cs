using System.ComponentModel.DataAnnotations;

namespace WebbApplication.Models;

public class SignInViewModel
{

    [Required(ErrorMessage = "This field is required.")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email", Prompt = "Enter email address")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "This field is required.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password", Prompt = "Enter password")]
    public string Password { get; set; } = null!;
    
    public bool RememberMe { get; set; }
}